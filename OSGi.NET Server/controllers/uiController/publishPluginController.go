package uiController

import (
	"time"

	"github.com/goinggo/tracelog"

	"OSGiServer/controllers/baseController"
	"OSGiServer/models"
	"OSGiServer/services/pluginService"
	"OSGiServer/services/pluginVersionService"
	"OSGiServer/utils/helper"
)

type PublishPluginController struct {
	baseController.BaseController
}

func (this *PublishPluginController) Get() {
	pluginid := this.Input().Get("pluginid")
	this.Data["pluginid"] = pluginid

	historyVersions, _ := pluginVersionService.GetPluginAllVersions(&this.Service, pluginid)
	this.Data["versions"] = historyVersions

	this.Data["active"] = "menu2"
	this.TplNames = "publishplugin.html"
}

func (this *PublishPluginController) Post() {
	pluginid := this.Input().Get("pluginid")
	plugin, err := pluginService.GetPlugin(&this.Service, pluginid)

	if err != nil {
		this.Ctx.Abort(404, "未找到插件!")
		return
	}

	plugin.UpdateTime = time.Now()

	var pulginversion models.PluginVersion
	pulginversion.PluginId = pluginid
	pulginversion.Version = this.Input().Get("version")
	pulginversion.CreateTime = time.Now()
	pulginversion.TraceInfo = this.Input().Get("traceinfo")

	tracelog.Info("保存接口文件", "Post", "开始保存")
	pif, pih, _ := this.GetFile("plugininterface")
	if pif != nil {
		pif.Close()
		tracelog.Info("保存接口文档", "Post", pih.Filename)
		helper.CreatDirAll("static/plugin/"+plugin.PluginId+"/versions/"+pulginversion.Version, "/")
		this.SaveToFile("plugininterface", "./static/plugin/"+plugin.PluginId+"/versions/"+pulginversion.Version+"/interface.zip")
		pulginversion.PluginInterface = "static/plugin/" + plugin.PluginId + "/versions/" + pulginversion.Version + "/interface.zip"
	}
	tracelog.Info("保存接口文件", "Post", "结束保存")

	tracelog.Info("保存插件文件", "Post", "开始保存")
	ppf, pph, _ := this.GetFile("pluginpackage")
	if ppf != nil {
		ppf.Close()
		tracelog.Info("保存接口文档", "Post", pph.Filename)
		helper.CreatDirAll("static/plugin/"+plugin.PluginId+"/versions/"+pulginversion.Version, "/")
		this.SaveToFile("pluginpackage", "./static/plugin/"+plugin.PluginId+"/versions/"+pulginversion.Version+"/bundle.zip")
		pulginversion.PluginInterface = "static/plugin/" + plugin.PluginId + "/versions/" + pulginversion.Version + "/bundle.zip"
	}
	tracelog.Info("保存插件文件", "Post", "结束保存")

	pluginVersionService.AddPluginVersion(&this.Service, &pulginversion)

	pluginService.AddPlugin(&this.Service, plugin)

	tracelog.Info("保存版本信息", "Post", "结束保存")
	this.Ctx.Redirect(302, "/publishplugin?pluginid="+pluginid+"&result=success")
}

func (this *PublishPluginController) DeleteVersion() {
	pluginid := this.Input().Get("pluginid")
	version := this.Input().Get("version")
	pluginVersionService.DeletePluginVersion(&this.Service, pluginid, version)
	this.Ctx.Redirect(302, "/publishplugin?pluginid="+pluginid)
}
