package uiController

import (
	"OSGiServer/controllers/baseController"
)

type TelnetController struct {
	baseController.BaseController
}

func (this *TelnetController) Get() {
	ip := this.Input().Get("ip")
	port := this.Input().Get("port")
	mac := this.Input().Get("mac")
	this.Data["active"] = "menu1"
	this.Data["Ip"] = ip
	this.Data["Port"] = port
	this.Data["Mac"] = mac
	this.TplNames = "telnet.html"
}
