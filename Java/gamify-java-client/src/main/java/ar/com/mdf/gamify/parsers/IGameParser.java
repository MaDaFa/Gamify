package ar.com.mdf.gamify.parsers;

public interface IGameParser<T> {
	public T parseToObject(String json, Class<T> classe);
	public Object parseToObject(String json);
	public String parseToString(T obj);
}
