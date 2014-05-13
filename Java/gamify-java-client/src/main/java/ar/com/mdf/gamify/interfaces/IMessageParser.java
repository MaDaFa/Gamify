package ar.com.mdf.gamify.interfaces;

public interface IMessageParser<T> extends IMessageParserBase {	
	public void addObserver(IObserver<T> observer);
}
