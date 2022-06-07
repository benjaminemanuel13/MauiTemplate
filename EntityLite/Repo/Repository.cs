using EntityLite.Attributes;
using EntityLite.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EntityLite.Repos
{
    public class Repository<T> where T : IBase, new()
    {
        private SQLiteConnection _conn;
        private readonly string _dbPath;

        public Repository(string dbPath)
        {
            _dbPath = dbPath;

            Init();
        }

        private void Init()
        {
            if (_conn != null)
            {
                return;
            }

            _conn = new SQLiteConnection(_dbPath);
            _conn.CreateTable<T>();
        }

        public int Create(T obj)
        {
            Type type = typeof(T);

            var properties = type.GetProperties();

            foreach (var info in properties)
            {
                foreach (var att in info.GetCustomAttributes(false))
                {
                    if (att is ForeignKeyAttribute)
                    {
                        var foreignKey = (ForeignKeyAttribute)att;
                        string name = foreignKey.Property;

                        var into = info.PropertyType;

                        MethodInfo[] method = typeof(SQLiteConnection).GetMethods().Where(x => x.Name == "Insert").ToArray();
                        
                        var newOne = info.GetValue(obj, null);

                        method[0].Invoke(_conn, new object[] { newOne });

                        PropertyInfo subInfos = obj.GetType().GetProperty(name);
                        PropertyInfo id = newOne.GetType().GetProperty("Id");
                        var theId = id.GetValue(newOne, null);
                        subInfos.SetValue(obj, theId, null);
                    }
                }
            }

            _conn.Insert(obj);

            return obj.Id;
        }

        public bool Update(T obj)
        {
            Type type = typeof(T);

            var properties = type.GetProperties();

            foreach (var info in properties)
            {
                foreach (var att in info.GetCustomAttributes(false))
                {
                    if (att is ForeignKeyAttribute)
                    {
                        var foreignKey = (ForeignKeyAttribute)att;
                        string name = foreignKey.Property;

                        var into = info.PropertyType;

                        MethodInfo[] method = typeof(SQLiteConnection).GetMethods().Where(x => x.Name == "Update").ToArray();

                        var newOne = info.GetValue(obj, null);

                        method[0].Invoke(_conn, new object[] { newOne });
                    }
                }
            }

            return _conn.Update(obj) > 0;
        }

        public bool Delete(T obj)
        {
            return _conn.Delete<T>(obj) > 0;
        }

        public T Get(int id)
        {
            var item = _conn.Table<T>().FirstOrDefault(x => x.Id == id);

            Type type = typeof(T);

            var properties = type.GetProperties();

            foreach (var info in properties)
            {
                foreach (var att in info.GetCustomAttributes(false))
                {
                    if (att is ForeignKeyAttribute)
                    { 
                        var foreignKey = (ForeignKeyAttribute)att;
                        string name = foreignKey.Property;

                        var foreignId = item.GetType().GetProperty(name).PropertyType;
                        var into = info.PropertyType;

                        MethodInfo method = typeof(SQLiteConnection).GetMethod("Table");
                        MethodInfo generic = method.MakeGenericMethod(into);
                        var ite = generic.Invoke(_conn, null);

                        MethodInfo[] meth = ite.GetType().GetMethods().Where(x => x.Name == "ToList").ToArray();
                        dynamic list = meth[0].Invoke(ite, null);

                        int i = (int)item.GetType().GetProperty(name).GetValue(item);

                        ParameterExpression B = Expression.Parameter(typeof(IBase));

                        IBase thisItem = null;
                        foreach(var it in list)
                        {
                            var ans = Expression.Lambda<Func<IBase, bool>>(Expression.Equal(Expression.PropertyOrField(B, "Id"), 
                            Expression.Constant(i)), B).Compile().Invoke(it as IBase);

                            if (ans)
                            {
                                thisItem = it;
                                break;
                            }
                        }

                        info.SetValue(item, thisItem);
                    }
                }
            }

            return item;
        }

        public List<T> GetRange(int start = 0, int length = 0)
        {
            if (length == 0)
            {
                return _conn.Table<T>().Skip(start).ToList();
            }
            else
            {
                return _conn.Table<T>().Skip(start).Take(length).ToList();
            }
        }
    }
}
