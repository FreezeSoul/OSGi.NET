package uiController

import (
	"OSGiServer/controllers/baseController"
	"OSGiServer/services/pluginService"
	"OSGiServer/services/pluginTypeService"
	"OSGiServer/services/pluginVersionService"
	"OSGiServer/utils"
)

type PluginDetailController struct {
	baseController.BaseController
}

func (this *PluginDetailController) Get() {
	pluginid := this.Input().Get("pluginid")
	this.Data["pluginid"] = pluginid

	plugin, err := pluginService.GetPlugin(&this.Service, pluginid)
	if err != nil {
		this.Ctx.Abort(404, "未找到插件!")
		return
	}

	plugintype, terr := pluginTypeService.GetPluginType(&this.Service, plugin.PluginTypeId)
	if terr == nil {
		this.Data["PluginTypeName"] = plugintype.PluginTypeName
	}

	historyVersions, _ := pluginVersionService.GetPluginAllVersions(&this.Service, pluginid)

	if len(historyVersions) > 0 {
		this.Data["historyVersions"] = historyVersions
		this.Data["LastVersion"] = historyVersions[len(historyVersions)-1].Version
	} else {
		this.Data["LastVersion"] = "<无版本>"
	}

	if plugin.PluginIsShare {
		this.Data["PluginKindName"] = "公共插件"
	} else {
		this.Data["PluginKindName"] = "项目插件"
	}

	if plugin.PluginImage == "" {
		plugin.PluginImage = "/static/images/plugin.gif"
	}

	this.Data["plugin"] = plugin
	this.Data["pluginDetail"] = utils.RawHTML(plugin.PluginDetail)

	backUrl := this.Ctx.Request.Referer()
	if backUrl == "" {
		backUrl = "/shareplugin"
	}
	this.Data["backurl"] = backUrl

	this.Data["active"] = "menu2"
	this.TplNames = "plugindetail.html"
}
