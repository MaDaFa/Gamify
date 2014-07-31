package ar.com.mdf.gamify.services;

import ar.com.mdf.gamify.objects.request.IRequestObject;

public interface IGameSender<T extends IRequestObject> {

	public void send(T request);
}
