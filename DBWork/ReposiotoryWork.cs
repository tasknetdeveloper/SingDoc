using Model;
using LoggerSpace;
using MySqlConnector;

namespace DBWork
{

    public class ReposiotoryWork 
    {
        private string connection = "";
        private Log log = new(true);
        public ReposiotoryWork(string connection)
        {
            this.connection = connection;
        }

        #region Get
        public Doc? GetDoc(Func<Doc, bool> query)
        {
            Doc? result = null;
            try
            {
                using (var r = new ApplicationContext(this.connection))
                {
                    if (r != null && r.Doc != null)
                        result = r.Doc.Where(query).FirstOrDefault();
                }
            }
            catch (MySqlException exp)
            {
                log.Error($"Message={exp.Message} InnerException={exp.InnerException}");
            }
            return result;
        }


        public Doc[]? GetDocs(Func<Doc, bool> query)
        {
            Doc[]? result = null;

            try
            {
                using (var r = new ApplicationContext(this.connection))
                {
                    if (r != null && r.Doc != null)
                    {
                        result = r.Doc.Where(query).ToArray();
                    }
                }
            }
            catch (MySqlException exp)
            {
                log.Error($"GetDocList/Message={exp.Message} InnerException={exp.InnerException}");
            }
            return result;
        }
        #endregion

        #region Add Update
        public bool Add(Doc model)
        {
            var result = false;
            if (model == null) return result;
            try
            {
                using (var r = new ApplicationContext(this.connection))
                {
                    if (r != null && r.Doc != null)
                    {
                        r.Doc.Add(model);
                        r.SaveChanges();
                        result = true;
                    }
                }
            }
            catch (MySqlException exp)
            {
                log.Error($"Add/Message={exp.Message} InnerException={exp.InnerException}");
            }
            return result;
        }
        #endregion

        #region Delete
        public bool Delete(Doc model)
        {
            var result = false;
            if (model == null) return result;
            try
            {
                using (var r = new ApplicationContext(this.connection))
                {
                    if (r != null && r.Doc != null)
                    {
                        var d = r.Doc.Where(x => x.id == model.id).FirstOrDefault();
                        if (d != null)
                        {
                            r.Doc.Remove(d);
                            r.SaveChanges();
                        }
                        result = true;
                    }
                }
            }
            catch (MySqlException exp)
            {
                log.Error($"Delete/Message={exp.Message} InnerException={exp.InnerException}");
            }
            return result;
        }        
        #endregion
    }
}
