package ar.com.mdf.gamify.objects.notification;

import com.google.gson.annotations.SerializedName;

public class GameMoveResultNotificationObject extends GameMoveNotificationObject {
	
	@SerializedName("Goods")
	private String goods;
	
	@SerializedName("Regulars")
	private String regulars;
	
	@SerializedName("Bads")
	private String bads;

	public String getGoods() {
		return goods;
	}

	public void setGoods(String goods) {
		this.goods = goods;
	}

	public String getRegulars() {
		return regulars;
	}

	public void setRegulars(String regulars) {
		this.regulars = regulars;
	}

	public String getBads() {
		return bads;
	}

	public void setBads(String bads) {
		this.bads = bads;
	}
	

}
