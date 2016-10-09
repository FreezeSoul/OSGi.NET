package uiController

import (
	"OSGiServer/controllers/baseController"
	"OSGiServer/services/pluginService"
	"OSGiServer/services/pluginVersionService"
)

type PluginVersionController struct {
	baseController.BaseController
}

func (this *PluginVersionController) Get() {
	pluginid := this.Input().Get("pluginid")
	version := this.Input().Get("version")
	this.Data["pluginid"] = pluginid

	_, err := pluginService.GetPlugin(&this.Service, pluginid)

	if err != nil {
		this.Ctx.Abort(404, "未找到插件!")
		return
	}

	historyVersions, _ := pluginVersionService.GetPluginAllVersions(&this.Service, pluginid)
	this.Data["versions"] = historyVersions

	currentVersion, _ := pluginVersionService.GetPluginVersion(&this.Service, pluginid, version)
	this.Data["version"] = currentVersion

	backUrl := this.Ctx.Request.Referer()
	if backUrl == "" {
		backUrl = "/plugindetail?pluginid=" + pluginid
	}
	this.Data["backurl"] = backUrl

	this.Data["active"] = "menu2"
	this.TplNames = "pluginversion.html"
}
