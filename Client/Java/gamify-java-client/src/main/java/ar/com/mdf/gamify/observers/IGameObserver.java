package ar.com.mdf.gamify.observers;

public interface IGameObserver<T> {
	public void notify(T notificationObject);
	
}
