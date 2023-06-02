

namespace RitaEngine.Base.Debug
{
    [SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential),SkipLocalsInit]
public static class Guard
{
    public static void IsStringEmpty(string value)
    {
        if ( string.IsNullOrEmpty(value))
            throw new Exception( string.Format($"SENTENCE IS EMPTY : {value}"));
    }

    public static void IsFileExist( string file)
    {
        if ( !System.IO.File.Exists(file )) 
            throw new Exception( string.Format($"FILE NOT EXIST : {file}"));
    }

    public static void IsNull<TEntity>(TEntity entity ) where TEntity : class 
    {
        if ( entity == null) 
            throw new Exception( string.Format($"{nameof(entity) } IS NULL"));
    }

    public unsafe static void VoidIsNull(void* entity ) 
    {
        if ( entity ==(void*)0) 
            throw new Exception( string.Format($"{nameof(entity) } IS NULL"));
    }
  
    public static void ThrowWhenConditionIsTrue(bool condition ,string message="")
    {
        if ( condition ) {
             throw new Exception( string.Format($"{condition } {(message=="" ? "":"=>") } {message}"));
        }
    }

    /// <summary>
    /// Vérifie une condition ; si la condition est false, affiche des messages qui montre la pile des appels.
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="message"></param>
    public static void Assert(bool condition ,string message="")
    {
        if ( condition ) {
            throw new Exception( string.Format($"{condition } {(message=="" ? "":"=>") } {message}"));
        }
    }

    // private static void Throw( bool conditionnal, string message , string errorCode)
    // {
    //     if(conditionnal){
    //         string error = string.IsNullOrEmpty(errorCode)? message :   errorCode.Replace("RITAENGINE.CORE.","") + " => "+ message; 
            
    //         Log.Critical( error );
    //         throw new CreateException<Exception>( error );
    //     }
    // }

    // [DebuggerHidden]
    // public static void Ensure(bool conditionnal, string message = "" ,int errorCode = 1 , [CallerArgumentExpression("errorCode")] string arg=""  )
    //     => Throw(  conditionnal,  message ,arg);

    // [DebuggerHidden]
    // public static void Guard(bool conditionnal, string message = "" ,int errorCode = 1 , [CallerArgumentExpression("errorCode")] string arg="" )
    //     => Throw(  conditionnal,  message ,arg);

    // [DebuggerHidden]
    // public static void Assert(bool conditionnal, string message = "" ,int errorCode = 1 , [CallerArgumentExpression("errorCode")] string arg="" )
    //     => Throw(  conditionnal,  message ,arg);



    // private static void For( Func<bool> predicate )
    // {
    //     if ( predicate.Invoke() )
    //         throw CreateException<Exception>($"For {predicate}");
    // }
    
    // private static TException CreateException<TException>( string message = null!  ) where TException : class , new()
    // {
    //     try
    //     {
    //         return  message == null!
    //                 ? new TException() 
    //                 : (TException)Activator.CreateInstance(typeof(TException), message)!;
    //     }
    //     catch (MissingMethodException)
    //     {
    //         return new TException();
    //     }
    // }

}


}
