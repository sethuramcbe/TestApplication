using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
  class Program
  {
    static void Main(string[] args)
    {
      var connection = new Connnection();
      connection.Start();
      Console.Read();
    }
  }

  /// <summary>
  /// The connnection.
  /// </summary>
  public class Connnection :IConnection
  {
   public Connnection()
   {
     this.State = new NotConnectedState(this);
   }

   public void Dispose()
    {
    }

    public void Start()
    {
      this.State.ChangeState(this);
    }

    public void Stop()
    {
      this.State.ChangeState(this);
    }

    public void Aborted()
    {
      this.State.ChangeState(this);
    }

    public IState<IConnection> State { get; set; }
  }
  /// <summary>
  /// The State interface.
  /// </summary>
  /// <typeparam name="T">
  /// </typeparam>
  public  interface IState<T>
  {
    /// <summary>
    /// The change state.
    /// </summary>
    /// <param name="owner">
    /// The owner.
    /// </param>
    void ChangeState(T owner);
  }


  public  abstract  class State<T> :IState<T>
  {

    protected State(T Owner )
    {
      
    }

    /// <summary>
    /// The change state.
    /// </summary>
    /// <param name="owner">
    /// The owner.
    /// </param>
    public void ChangeState(T owner)
    {
      OnChangeState(owner);
    }

    /// <summary>
    /// The on change state.
    /// </summary>
    /// <param name="Owner">
    /// The owner.
    /// </param>
    public abstract void OnChangeState(T Owner);
  }

  public interface IConnection : IDisposable
  {
    void Start( );

    void Stop();

    void Aborted();

    IState<IConnection> State { get; set; }
  }

  /// <summary>
  /// The Connected interface.
  /// </summary>
  public interface IConnected : IState<IConnection>
  {
  }

  /// <summary>
  /// The connected state.
  /// </summary>
  public class ConnectedState : State<IConnection>, IConnected
  {
    public ConnectedState(IConnection Owner)
      : base(Owner)
    {
    }

    public override void OnChangeState(IConnection Owner)
    {
      var state = new NotConnectedState(Owner);
      Owner.State = state;
    }
  }

  public class NotConnectedState : State<IConnection>
  {
    public NotConnectedState(IConnection Owner)
      : base(Owner)
    {
    }

    public override void OnChangeState(IConnection Owner)
    {
      var state = new InProgressState(Owner);
      Owner.State = state;
    }
  }

  public class InProgressState : State<IConnection>
  {
    public InProgressState(IConnection Owner)
      : base(Owner)
    {
    }

    public override void OnChangeState(IConnection Owner)
    {
      var state = new ConnectedState(Owner);
      Owner.State = state;
    }
  }

  /// <summary>
  /// The aborted state.
  /// </summary>
  public class AbortedState : State<IConnection>
  {
    public AbortedState(IConnection Owner)
      : base(Owner)
    {
    }

    public override void OnChangeState(IConnection Owner)
    {
      var state = new InProgressState(Owner);
      Owner.State = state;
    }
  }
}
