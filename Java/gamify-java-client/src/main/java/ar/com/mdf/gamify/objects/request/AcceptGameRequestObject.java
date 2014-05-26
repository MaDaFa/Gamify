package ar.com.mdf.gamify.objects.request;

import com.google.gson.annotations.SerializedName;

public class AcceptGameRequestObject extends SessionRequestObject {

	@SerializedName("AdditionalInformation")
	private String additionalInformation;

	public String getAdditionalInformation() {
		return additionalInformation;
	}

	public void setAdditionalInformation(String additionalInformation) {
		this.additionalInformation = additionalInformation;
	}

	
}
