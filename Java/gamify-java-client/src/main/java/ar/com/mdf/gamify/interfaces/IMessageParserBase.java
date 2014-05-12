package ar.com.mdf.gamify.interfaces;

import ar.com.mdf.gamify.enums.NotificationType;

public interface IMessageParserBase {

	public boolean canParse(NotificationType type);
	
	public boolean canParse(Class<?> type);
	
	public void notify(String message);

}


