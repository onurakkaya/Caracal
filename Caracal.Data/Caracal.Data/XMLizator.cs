using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Caracal.Data.Xmlizator
{
    // ReSharper disable once InconsistentNaming
    public delegate void XMLizatorItemAddedEventHandler(object sender, XMLizatorEventArgs e);
    public delegate void XMLizatorItemRemovedEventHandler(object sender, XMLizatorEventArgs e);
    public delegate void XMLizatorItemUpdatedEventHandler(object sender, XMLizatorEventArgs e);
    public delegate void XMLizatorFileChangedEventHandler(object sender, XMLizatorEventArgs e);
    public class XMLizator<T> where T : XMLizatorLayout
    {
        private string _fPath = string.Empty;
        private T _fLayout = default(T);
        Queue<KeyValuePair<T, string>> kayitSirasi = new Queue<KeyValuePair<T, string>>();
        Queue<KeyValuePair<string, string>> silmeSirasi = new Queue<KeyValuePair<string, string>>();
        //Thread sirayiIsle;
        protected CancellationTokenSource taskCancelSource = new CancellationTokenSource();
        private CancellationToken cancelToken;
        protected Task sirayiIsle = default(Task);
        private bool flagThread1 = true;
        private bool readOnly = false;
        private int _procQueueFreqSec = 0;

        //-----------------------------------------
        public event XMLizatorItemAddedEventHandler ItemAdded;
        public event XMLizatorItemUpdatedEventHandler ItemUpdated;
        public event XMLizatorItemRemovedEventHandler ItemRemoved;
        public event XMLizatorFileChangedEventHandler FileChanged;
        //-----------------------------------------

        public XMLizator(string path, T layout, int beklemeSuresi = 20, bool sadeceOkuma = false)
        {
            _fPath = path;
            _fLayout = layout;
            _procQueueFreqSec = beklemeSuresi;

            if (!sadeceOkuma)
            {
                cancelToken = taskCancelSource.Token;
                sirayiIsle = new Task(new Action(delegate
                    {
                        while (flagThread1)
                        {
                            while (kayitSirasi.GetEnumerator().MoveNext())
                            {
                                KeyValuePair<T, string> lpath = kayitSirasi.Dequeue();
                                if (!String.IsNullOrWhiteSpace(lpath.Value))
                                    while (!this.Save(lpath.Value, lpath.Key))
                                        Thread.Sleep(TimeSpan.FromMilliseconds(50));
                            }

                            while (silmeSirasi.GetEnumerator().MoveNext())
                            {
                                KeyValuePair<string, string> lpath = silmeSirasi.Dequeue();
                                if (!string.IsNullOrWhiteSpace(lpath.Value))
                                    while (!this.Delete(lpath.Value, lpath.Key))
                                        Thread.Sleep(TimeSpan.FromMilliseconds(50));
                            }
                            Thread.Sleep(TimeSpan.FromSeconds(beklemeSuresi));
                        }

                        taskCancelSource.Cancel();
                    }), cancelToken);
                //sirayiIsle = new Thread(new ThreadStart(delegate
                //{

                //}));
                sirayiIsle.Start();
            }
            else
            {
                readOnly = sadeceOkuma;
            }
        }

        /// <summary>
        /// Klasör ağacındaki eksik klasörleri oluşturur
        /// </summary>
        /// <param name="dirPath">taranacak dosya konumu full path gönderilmelidir. dosya adı dikkate alınmaz.</param>
        /// <returns>boolean tipte başarılı / başarısız yanıtı</returns>
        private Boolean AutoPathRepair(string dirPath)
        {
            try
            {
                String CheckLoc = String.Empty;
                String[] location = dirPath.Split('\\');
                for (int i = 0; i < location.Length - 1; i++)
                {
                    CheckLoc = CheckLoc.Insert(CheckLoc.Length, String.Format("{0}\\", location[i]));
                    if (!Directory.Exists(CheckLoc))
                    {
                        Directory.CreateDirectory(CheckLoc);
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Dosyaya başka bir stream yada program tarafından şuanda yazılıyor mu kontrol eder.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        /// <summary>
        /// XMLizator Genel nod kaydedici tüm overloadları bu metod işleme koyar.
        /// </summary>
        /// <param name="path">kayıt konumu klasör ağacının eksik parçaları oto. oluşturulur.(oto path repair)</param>
        /// <param name="layout">Oluşturulan class tipinde layout nesnesi t parametresi ile generic olarak girer</param>
        private bool Save(string path, T layout)
        {
            XMLizatorEventArgs eParam = new XMLizatorEventArgs();
            bool isUpdateProc = false;
            eParam.Path = path;
            if (!readOnly)
            {
                dynamic sDocument = null;
                etiket:
                if (File.Exists(path))
                {
                    try
                    {
                        var doc = XElement.Load(path);
                        var target = doc
                            .Elements()
                            .Where(e => e.Value == layout.Kod);
                        if (target.ToList().Count > 0)
                        {
                            XElement kodElement = target.First();
                            var pInfo = layout.GetType().GetProperties().ToList();/*.Where(
                                        sr => sr.PropertyType.IsEquivalentTo(typeof(string)));*/

                            pInfo.ToList().ForEach(sr =>
                            {
                                if (sr.Name != "Kod")
                                {
                                    object str = null;
                                    str = sr.GetValue(layout, null);
                                    kodElement.SetAttributeValue(sr.Name, str.ToString());
                                }
                            });
                            isUpdateProc = true;
                        }
                        else
                        {
                            XNode RootElement = doc.FirstNode;
                            var pInfo = layout.GetType().GetProperties().ToList();/*.Where(
                                        sr => sr.PropertyType.IsEquivalentTo(typeof(string))
                                        );*/
                            XElement kodElement = default(XElement);
                            List<PropertyInfo> pXinfo = pInfo.ToList();
                            var pxKodInfo = pXinfo.Where(s => s.Name == "Kod").ToList();

                            if (pxKodInfo.Count > 0)
                            {
                                PropertyInfo p = pxKodInfo[0];
                                kodElement = new XElement(p.Name, p.GetValue(layout, null));
                                RootElement.AddAfterSelf(kodElement);
                                pXinfo.Remove(p);
                            }



                            pXinfo.ForEach(sr =>
                             {
                                 object str = null;
                                 str = sr.GetValue(layout, null);
                                 kodElement.Add(new XAttribute(sr.Name, str.ToString()));
                             });
                            isUpdateProc = false;
                        }
                        sDocument = doc;
                        eParam.Document = null;
                    }
                    catch (XmlException)
                    {
                        File.Delete(path);
                        goto etiket;
                    }
                }
                else
                {
                    XDocument dokuman = new XDocument();
                    XElement RootElement = new XElement("Layout");
                    dokuman.Add(RootElement);
                    if (this.AutoPathRepair(path))
                    {
                        var pInfo = layout.GetType().GetProperties().ToList();/*.Where(
                                 sr => sr.PropertyType.IsEquivalentTo(typeof(string))
                                 );*/
                        XElement kodElement = default(XElement);

                        List<PropertyInfo> pXinfo = pInfo.ToList();
                        var pxKodInfo = pXinfo.Where(s => s.Name == "Kod").ToList();

                        if (pxKodInfo.Count > 0)
                        {
                            PropertyInfo p = pxKodInfo[0];
                            kodElement = new XElement(p.Name, p.GetValue(layout, null));
                            RootElement.Add(kodElement);
                            pXinfo.Remove(p);
                        }

                        pXinfo.ToList().ForEach(sr =>
                        {
                            object str = null;
                            str = sr.GetValue(layout, null);
                            kodElement.Add(new XAttribute(sr.Name, str.ToString()));
                        });
                    }
                    sDocument = dokuman;
                    eParam.Document = dokuman;
                }
                sDocument.Save(path);
                if (ItemAdded != null && !isUpdateProc)
                    ItemAdded(layout, eParam);
                else if (ItemUpdated != null && isUpdateProc)
                    ItemUpdated(layout, eParam);
                else if (FileChanged != null)
                    FileChanged(layout, eParam);
                return true;
            }
            else
            {
                Exception ex = new Exception("Sınıf readonly olarak işaretlenmiş thread çalışmadığından kayıt yapılamaz.");
                throw ex;
            }
        }
        /// <summary>
        /// XMLizator Genel nod okuyucu tüm overloadları bu metod işleme koyar.
        /// </summary>
        /// <param name="path">okuma konumu dosya adı ile birlikte</param>
        /// <param name="code">aranan data kodu/id</param>
        /// <returns>XMLizator yapılan class tipinden generic bir T nesnesi</returns>
        private T Read(string path = default(string), string code = default(string))
        {
            T _layout = Activator.CreateInstance<T>();
            if (File.Exists(path))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                var pInfo = _layout.GetType().GetProperties().ToList();/*.Where(
                       sr => sr.PropertyType.IsEquivalentTo(typeof(string)));*/

                List<XmlNode> result = doc.ChildNodes[1].ChildNodes.Cast<XmlNode>().ToList();
                List<XmlNode> xNode = result.Where(sr => sr.FirstChild != null && sr.FirstChild.Value != null && sr.FirstChild.Value == code).ToList();
                XmlNode node = default(XmlNode);
                if (xNode.Count > 0)
                {
                    node = xNode.First();
                    _layout.Kod = node.FirstChild.Value;
                    node.Attributes.Cast<XmlAttribute>().ToList().ForEach(nd =>
                 {
                     if (nd.Value != null)
                     {
                         var info = pInfo.Where(sr => sr.Name == nd.Name);
                         if (info.FirstOrDefault() != null)
                         {
                             var parseReadyValue = Convert.ChangeType(nd.Value, info.First().PropertyType);
                             info.First().SetValue(_layout, parseReadyValue, null);
                         }
                     }
                 });
                }
            }
            else
                return default(T);
            return (T)Convert.ChangeType(_layout, typeof(T));
        }
        /// <summary>
        /// Dokümandaki tüm nodları okur
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private List<T> ReadAll(string path = default(string))
        {
            T _layout = Activator.CreateInstance<T>();
            if (this.AutoPathRepair(path) && File.Exists(path))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                var pInfo = _layout.GetType().GetProperties().ToList().Where(
                    sr => sr.PropertyType.IsEquivalentTo(typeof(string)));
                List<XmlNode> result = doc.ChildNodes[1].ChildNodes.Cast<XmlNode>().ToList();
                List<XmlNode> xNode = result.Where(sr => sr.FirstChild != null && sr.FirstChild.Value != null).ToList();


                List<T> nodeList = new List<T>();
                XmlNode node = default(XmlNode);
                if (xNode.Count > 0)
                {
                    xNode.ForEach(lnode =>
                    {
                        node = lnode;
                        T temp = Activator.CreateInstance<T>();
                        temp.Kod = node.FirstChild.Value;
                        node.Attributes.Cast<XmlAttribute>().ToList().ForEach(nd =>
                        {
                            if (nd.Value != null)
                            {
                                var info = pInfo.Where(sr => sr.Name == nd.Name);
                                info.First().SetValue(temp, nd.Value, null);
                            }
                        });
                        nodeList.Add((T)temp);
                    });
                    return nodeList;
                }
            }

            return null;
        }
        /// <summary>
        /// XMLizator Genel nod silici tüm overloadları bu metod işleme koyar.
        /// </summary>
        /// <param name="path">silincek datayı içeren dosya konumu </param>
        /// <param name="code">silinecen dosya kodu / id si</param>
        private bool Delete(string path = default(string), string code = default(string))
        {
            XMLizatorEventArgs eParam = new XMLizatorEventArgs();
            eParam.Path = path;
            if (!readOnly)
            {
                if (File.Exists(path))
                {
                    var doc = XElement.Load(path);
                    doc.Elements().Where(e => e.Value == code).Remove();

                    doc.Save(path);

                    if (ItemRemoved != null)
                        ItemRemoved(code, eParam);
                    else if (FileChanged != null)
                        FileChanged(code, eParam);

                    if (doc.Elements().Count() < 1)
                        File.Delete(path);
                    return true;
                }
                return false;
            }
            else
            {
                Exception ex = new Exception("Sınıf readonly olarak işaretlenmiş thread çalışmadığından kayıt yapılamaz.");
                throw ex;
            }
        }

        /// <summary>
        /// XMLizator kuyruk işleme threadini durdurmak için kullanılır.
        /// </summary>
        /// <returns>işlem sonucunu döner</returns>
        private bool Abort()
        {
            flagThread1 = false;
            return true;
        }

        #region Kaydet-OVERLOADS

        public void Kaydet(T layout)
        {
            kayitSirasi.Enqueue(new KeyValuePair<T, string>(layout, _fPath));
        }
        public void Kaydet(string path, T layout)
        {
            kayitSirasi.Enqueue(new KeyValuePair<T, string>(layout, path));
        }
        public void Kaydet()
        {
            kayitSirasi.Enqueue(new KeyValuePair<T, string>(_fLayout, _fPath));
        }

        #endregion
        #region Oku-OVERLOADS
        public T Oku(string kod)
        {
            //Hemen öncesinde veri kaydedildiyse/silindiyse beklemesüresi : 0 olsa bile xmlizatörün yazmak için 100ms ye ihtiyacı var.
            if (_procQueueFreqSec == 0) Thread.Sleep(TimeSpan.FromMilliseconds(150));

            return this.Read(path: _fPath, code: kod);
        }
        public T Oku(string path, string kod)
        {
            //Hemen öncesinde veri kaydedildiyse/silindiyse beklemesüresi : 0 olsa bile xmlizatörün yazmak için 100ms ye ihtiyacı var.
            if (_procQueueFreqSec == 0) Thread.Sleep(TimeSpan.FromMilliseconds(150));

            return this.Read(path: path, code: kod);
        }
        public List<T> HepsiniOku(string path)
        {
            //Hemen öncesinde veri kaydedildiyse/silindiyse beklemesüresi : 0 olsa bile xmlizatörün yazmak için 100ms ye ihtiyacı var.
            if (_procQueueFreqSec == 0) Thread.Sleep(TimeSpan.FromMilliseconds(150));

            return this.ReadAll(path: path);
        }
        #endregion
        #region Sil-OVERLOADS
        public void Sil(string kod)
        {
            silmeSirasi.Enqueue(new KeyValuePair<string, string>(kod, _fPath));
        }
        public void Sil(string path, string kod)
        {
            silmeSirasi.Enqueue(new KeyValuePair<string, string>(kod, path));
        }
        #endregion
        #region Durdur-OVERLOADS
        public void Durdur()
        {
            Abort();
        }
        #endregion


    }
    public interface XMLizatorLayout
    {
        string Kod { get; set; }
    }
}
