

namespace RitaEngine.Resources;

using System.Collections.Generic;
using System.Linq.Expressions;
using RitaEngine.Resources.Storage.UltraLiteDB;

public static class StorageHelper
{
    public static int Count<T>(string database, string collection)
    {
        using var db = new UltraLiteDatabase(database);
        return db.GetCollection<T>(collection).Count();
    }

    public static bool CreateDatabase(string databasename)
    {
        var db = new UltraLiteDatabase(databasename);

        if (db == null)
            return false;

        db.Dispose();
        return true ;
    }

    public static bool CreateIndex<T, T2>(string database, string collection, string fieldToIndexed, bool unique = false)
    {
        using var db = new UltraLiteDatabase(database);
        bool result = db.GetCollection<T>(collection).EnsureIndex(fieldToIndexed, unique);
        db.Dispose();
        return result;
    }

    public static bool DeleteById<T>(string database, string collection, int id)
    {
        using var db = new UltraLiteDatabase(database);
        bool result = db.GetCollection<T>(collection).Delete(new BsonValue(id));
        db.Dispose();
        return result;
    }

    public static IEnumerable<T> GetAll<T>(string database, string collection)
    {
        using var db = new UltraLiteDatabase(database);
        var result = db.GetCollection<T>(collection).FindAll();
        db.Dispose();
        return result;
    }

    public static T GetById<T>(string database, string collection, int id)
    {
        using var db = new UltraLiteDatabase(database);
        T result =db.GetCollection<T>(collection).FindById(new BsonValue(id));
        db.Dispose();
        return  result;
    }

        public static T Insert<T>(string database, string collection, T element, bool autoIncrement = true)
    {
        
        if (element == null)  throw new Exception("Elemnt not must be null");    
        
        using var db = new UltraLiteDatabase(database);

        if (!autoIncrement) // unique row
        {
            _ = db.GetCollection<T>(collection).Upsert(element);
        }
        else // auto increment
        {
            _ = db.GetCollection<T>(collection).Insert(element);
        }
        T result =  (!autoIncrement) ? element : db.GetCollection<T>(collection).FindOne(Query.All(Query.Descending));
        db.Dispose();
        return result;
    }
    
    public static bool Update<T>(string database, string collection, T element)
    {
        using var db = new UltraLiteDatabase(database);
        bool result = db.GetCollection<T>(collection).Update(element);
        db.Dispose();
        return result;
    }

    
    // public static int CountMany<T>(string database, string collection, Expression<Func<T, bool>> predicate)
    // {
    //     using var db = new UltraLiteDatabase(database);
    //     return db.GetCollection<T>(collection).Count(predicate);
    // }

    // public static int DeleteAll<T>(string database, string collection)
    // {
    //     using var db = new UltraLiteDatabase(database);
    //     return db.GetCollection<T>(collection).DeleteAll();
    // }

    // public static int DeleteMany<T>(string database, string collection, Expression<Func<T, bool>> predicate)
    // {
    //     using var db = new UltraLiteDatabase(database);
    //     return db.GetCollection<T>(collection).DeleteMany(predicate);
    // }

    // public static bool Exists<T>(string database, string collection, Expression<Func<T, bool>> predicate) //TODO Storage Helper replace Expression with QUery
    // {
    //     using var db = new UltraLiteDatabase(database); 
    //     return db.GetCollection<T>(collection).Exists(predicate);
    // }

    // public static IEnumerable<T> GetMany<T>(string database, string collection, Expression<Func<T, bool>> predicate, int skip, int limit)
    // {
    //     using var db = new UltraLiteDatabase(database);
    //     return db.GetCollection<T>(collection).Query().Where(predicate).Skip(skip).Limit(limit).ToArray();
    // }

    // public static T GetOne<T>(string database, string collection, Expression<Func<T, bool>> predicate)
    // {
    //     using var db = new UltraLiteDatabase(database);
    //     return db.GetCollection<T>(collection).FindOne(predicate);
    // }

    // public static T LastItem<T>(string database, string collection)
    // {
    //     using var db = new UltraLiteDatabase(database);
    //     return db.GetCollection<T>(collection).Query().Skip(db.GetCollection<T>(collection).Count() - 1).FirstOrDefault();
    // }


    // public static void Test<T>(string database, string collection, T element)
    // {
    //     var result =  new UltraLiteDatabase(database).GetCollection<T>(collection) ;
    // }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public static void LoadFile(string database, string id, string tmpPath)
    // {
    //     using var db = new UltraLiteDatabase(database);
    //     // Get file storage with Int Id
    //     var storage = db.GetStorage<string>();
    //     // Upload a file from file system to database
    //     storage.Download(id, tmpPath, true);
    // }

    // public static void SaveFile(string database, string id, string file)
    // {
    //     using var db = new UltraLiteDatabase(database);
    //     // Get file storage with Int Id
    //     var storage = db.GetStorage<string>();
    //     // Upload a file from file system to database
    //     storage.Upload(id, file);
    // }
}


