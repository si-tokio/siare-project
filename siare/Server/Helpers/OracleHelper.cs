using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace siare.Server.Helpers
{
  /// <summary>
  /// Oracle Helper
  /// </summary>
  public static class OracleHelper
  {
    static OracleHelper()
    {
      DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    /// <summary>
    /// 接続文字列取得処理
    /// </summary>
    /// <returns>接続文字列</returns>
    private static string GetConnectionString()
    {
      var user = Environment.GetEnvironmentVariable("DB_USER") ?? "default_user";
      var pass = Environment.GetEnvironmentVariable("DB_PASS") ?? "default_pass";
      var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost/xepdb1";

      return $"user id={user};password={pass};data source={host}";
    }

    /// <summary>
    /// 検索１件（条件なし）
    /// </summary>
    public static TReturn GetOne<TReturn>(string sql) => GetOne<TReturn, object>(sql, null);

    /// <summary>
    /// 検索１件（条件付き）
    /// </summary>
    public static TReturn GetOne<TReturn, TParameter>(string sql, TParameter? parameter)
    {
      TReturn result;

      using (var con = new OracleConnection(GetConnectionString()))
      {
        try
        {
          con.Open();
          result = con.QueryFirstOrDefault<TReturn>(sql, parameter);
        }
        catch
        {
          throw;
        }
        finally
        {
          con.Close();
        }
      }

      return result;
    }

    /// <summary>
    /// 検索複数件（条件なし）
    /// </summary>
    public static List<TReturn> Get<TReturn>(string sql) => Get<TReturn, object>(sql, null);

    /// <summary>
    /// 検索複数件（条件付き）
    /// </summary>
    public static List<TReturn> Get<TReturn, TParameter>(string sql, TParameter? parameter)
    {
      List<TReturn> result;

      using (var con = new OracleConnection(GetConnectionString()))
      {
        try
        {
          con.Open();
          result = con.Query<TReturn>(sql, parameter).ToList();
        }
        catch
        {
          throw;
        }
        finally
        {
          con.Close();
        }
      }

      return result;
    }

    /// <summary>
    /// SQL実行（パラメータなし）
    /// </summary>
    public static int Execute(string sql)
    {
      int result = 0;

      using (var con = new OracleConnection(GetConnectionString()))
      {
        try
        {
          con.Open();
          result = con.Execute(sql);
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
        finally
        {
          con.Close();
        }
      }

      return result;
    }

    /// <summary>
    /// SQL実行
    /// </summary>
    public static int Execute<T>(string sql, T parameter)
    {
      List<T> p = new List<T> { parameter };
      return Execute<T>(sql, p);
    }

    /// <summary>
    /// SQL実行（複数回）
    /// </summary>
    public static int Execute<T>(string sql, IEnumerable<T> parameters)
    {
      int result = 0;

      using (var con = new OracleConnection(GetConnectionString()))
      {
        try
        {
          con.Open();
          if (parameters == null)
          {
            result = con.Execute(sql);
          }
          else
          {
            foreach (var parameter in parameters)
            {
              result += con.Execute(sql, parameter);
            }
          }
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
        finally
        {
          con.Close();
        }
      }

      return result;
    }

    /// <summary>
    /// SQL実行（複数レコードを一括で）
    /// </summary>
    public static int Execute(string sql, List<OracleParameter> parameters, int arrayBindCount)
    {
      int result = 0;

      using (var con = new OracleConnection(GetConnectionString()))
      using (var cmd = new OracleCommand(sql, con))
      {
        try
        {
          con.Open();
          cmd.ArrayBindCount = arrayBindCount;
          cmd.BindByName = true;
          cmd.Parameters.AddRange(parameters.ToArray());
          result = cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
        finally
        {
          con.Close();
        }
      }

      return result;
    }

    /// <summary>
    /// ストアド実行
    /// </summary>
    public static T ExecuteStoredProcedure<T>(string storedProcedureName, DynamicParameters parameters, string returnName)
    {
      using (var con = new OracleConnection(GetConnectionString()))
      {
        try
        {
          con.Open();
          con.Execute(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);
          T result = parameters.Get<T>(returnName);
          return result;
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
        finally
        {
          con.Close();
        }
      }
    }
  }
}
