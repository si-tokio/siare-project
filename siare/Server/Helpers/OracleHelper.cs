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
    public static Task<TReturn> GetOneAsync<TReturn>(string sql) => GetOneAsync<TReturn, object>(sql, null);

    /// <summary>
    /// 検索１件（条件付き）
    /// </summary>
    public static async Task<TReturn> GetOneAsync<TReturn, TParameter>(string sql, TParameter? parameter)
    {
      TReturn result;

      using (var con = new OracleConnection(GetConnectionString()))
      {
        try
        {
          await con.OpenAsync();
          result = await con.QueryFirstOrDefaultAsync<TReturn>(sql, parameter);
        }
        catch
        {
          throw;
        }
        finally
        {
          await con.CloseAsync();
        }
      }

      return result;
    }

    /// <summary>
    /// 検索複数件（条件なし）
    /// </summary>
    public static Task<List<TReturn>> GetAsync<TReturn>(string sql) => GetAsync<TReturn, object>(sql, null);

    /// <summary>
    /// 検索複数件（条件付き）
    /// </summary>
    public static async Task<List<TReturn>> GetAsync<TReturn, TParameter>(string sql, TParameter? parameter)
    {
      List<TReturn> result;

      using (var con = new OracleConnection(GetConnectionString()))
      {
        try
        {
          await con.OpenAsync();
          var r = await con.QueryAsync<TReturn>(sql, parameter);
          result = r.ToList();
        }
        catch
        {
          throw;
        }
        finally
        {
          await con.CloseAsync();
        }
      }

      return result;
    }

    /// <summary>
    /// SQL実行（パラメータなし）
    /// </summary>
    public static async Task<int> ExecuteAsync(string sql)
    {
      int result = 0;

      using (var con = new OracleConnection(GetConnectionString()))
      {
        try
        {
          await con.OpenAsync();
          result = await con.ExecuteAsync(sql);
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
        finally
        {
          await con.CloseAsync();
        }
      }

      return result;
    }

    /// <summary>
    /// SQL実行
    /// </summary>
    public static async Task<int> ExecuteAsync<T>(string sql, T parameter)
    {
      List<T> p = new List<T> { parameter };
      return await ExecuteAsync<T>(sql, p);
    }

    /// <summary>
    /// SQL実行（複数回）
    /// </summary>
    public static async Task<int> ExecuteAsync<T>(string sql, IEnumerable<T> parameters)
    {
      int result = 0;

      using (var con = new OracleConnection(GetConnectionString()))
      {
        try
        {
          await con.OpenAsync();
          if (parameters == null)
          {
            result = await con.ExecuteAsync(sql);
          }
          else
          {
            foreach (var parameter in parameters)
            {
              result += await con.ExecuteAsync(sql, parameter);
            }
          }
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
        finally
        {
          await con.CloseAsync();
        }
      }

      return result;
    }

    /// <summary>
    /// SQL実行（複数レコードを一括で）
    /// </summary>
    public static async Task<int> ExecuteAsync(string sql, List<OracleParameter> parameters, int arrayBindCount)
    {
      int result = 0;

      using (var con = new OracleConnection(GetConnectionString()))
      using (var cmd = new OracleCommand(sql, con))
      {
        try
        {
          await con.OpenAsync();
          cmd.ArrayBindCount = arrayBindCount;
          cmd.BindByName = true;
          cmd.Parameters.AddRange(parameters.ToArray());
          result = await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
        finally
        {
          await con.CloseAsync();
        }
      }

      return result;
    }

    /// <summary>
    /// ストアド実行
    /// </summary>
    public static async Task<T> ExecuteStoredProcedureAsync<T>(string storedProcedureName, DynamicParameters parameters, string returnName)
    {
      using (var con = new OracleConnection(GetConnectionString()))
      {
        try
        {
          await con.OpenAsync();
          await con.ExecuteAsync(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);
          T result = parameters.Get<T>(returnName);
          return result;
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
        finally
        {
          await con.CloseAsync();
        }
      }
    }
  }
}
