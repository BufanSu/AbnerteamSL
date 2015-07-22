using System.Linq;
using System.Collections.Generic;
using System.Text;
namespace AbnerTeamSL.Session
{
    /// <summary>
    /// 会话状态Session管理
    /// </summary>
    public class SessionManager
    {
      /// <summary>
      /// 会话状态Session数据集合
      /// </summary>
      private List<SessionInfo> listSession = null;

      /// <summary>
      /// 获取会话状态集合的项数
      /// </summary>
      public int Count
      {
        get
        {
          if (listSession != null)
            return listSession.Count;
          else
            return 0;
        }
      }

      #region 共有方法
      /// <summary>
      /// 无参数构造函数
      /// </summary>
      public SessionManager()
      {
        listSession = new List<SessionInfo>();
      }

      /// <summary>
      /// 按名称获取或设置会话的值
      /// </summary>
      /// <param name="SessionName">名称</param>
      /// <returns></returns>
      public object this[string SessionName]
      {
        get
        {
          SessionInfo si = listSession.FirstOrDefault(info => info.SessionName == SessionName);
          if (si == null)
            return null;
          else
            return si.SessionValue;
        }
        set
        {
          Add(SessionName, value);
        }
      }

      /// <summary>
      /// 向会话状态集合添加一个新项
      /// </summary>
      /// <param name="name">名称</param>
      /// <param name="value">值</param>
      public void Add(string name, object value)
      {
        //判断name是否存在,不存在就新增,存在就修改其对应的值
        SessionInfo si = listSession.FirstOrDefault(info => info.SessionName == name);
        if (si == null)
        {
          listSession.Add(new SessionInfo { SessionName = name, SessionValue = value });
        }
        else
        {
          si.SessionValue = value;
        }
      }

      /// <summary>
      /// 删除会话状态集合中的项
      /// </summary>
      /// <param name="name">名称</param>
      public void Remove(string name)
      {
        SessionInfo si = listSession.FirstOrDefault(info => info.SessionName == name);
        listSession.Remove(si);
      }

      /// <summary>
      /// 从会话状态集合中移除所有的项
      /// </summary>
      public void Clear()
      {
        listSession.Clear();
      }

      /// <summary>
      /// 重写ToString方法, 返回会话状态集合所有项的名称
      /// </summary>
      /// <returns></returns>
      public override string ToString()
      {
        StringBuilder strName = new StringBuilder();
        foreach (SessionInfo info in listSession)
        {
          strName.AppendFormat(",{0}", info.SessionName);
        }
        return strName.ToString().Substring(1);
      }
      #endregion
    }

    /// <summary>
    /// 会话状态Session实体
    /// </summary>
    public class SessionInfo
    {
      /// <summary>
      /// Session名称
      /// </summary>
      public string SessionName { get; set; }

      /// <summary>
      /// Session值
      /// </summary>
      public object SessionValue { get; set; }
    }
  
}
