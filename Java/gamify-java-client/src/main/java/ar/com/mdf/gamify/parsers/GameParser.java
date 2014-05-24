package ar.com.mdf.gamify.parsers;

import com.google.gson.Gson;

public class GameParser<T> implements IGameParser<T> {
	private static Gson gson = new Gson();


	public T parseToObject(String json, Class<T> classe) {
		return gson.fromJson(json, classe);   
	}
	
	public Object parseToObject(String json) {
		return gson.fromJson(json, Object.class);   
	}

	public String parseToString(T obj) {
		return gson.toJson(obj); 
	}
	
	
	

}
