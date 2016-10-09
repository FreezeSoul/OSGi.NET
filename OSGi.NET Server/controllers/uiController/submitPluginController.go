package uiController

import (
	"time"

	"github.com/goinggo/tracelog"
	"github.com/nu7hatch/gouuid"

	"OSGiServer/controllers/baseController"
	"OSGiServer/models"
	"OSGiServer/services/pluginService"
	"OSGiServer/services/pluginTypeService"
	"OSGiServer/services/projectPluginMapService"
	"OSGiServer/services/projectService"
	"OSGiServer/utils"
	"OSGiServer/utils/helper"
)

type SubmitPluginController struct {
	baseController.BaseController
}

//获取插件
func (this *SubmitPluginController) Get() {
	//获取所有插件类型
	types, _ := pluginTypeService.GetAllPluginTypes(&this.Service)
	this.Data["types"] = types

	projects, _ := projectService.GetAllProjects(&this.Service)
	this.Data["projects"] = projects

	pluginid := this.Input().Get("pluginid")
	this.Data["plugintypeid"] = ""
	this.Data["pluginisshare"] = "1"
	this.Data["projectid"] = "" //插件所属项目，仅项目插件才有
	this.Data["pluginimage"] = utils.RawJS("/static/images/plugin.gif")
	if pluginid != "" {
		//获取指定插件
		plugin, err := pluginService.GetPlugin(&this.Service, pluginid)
		if err == nil {
			this.Data["pluginid"] = plugin.PluginId
			this.Data["pluginname"] = plugin.PluginName
			this.Data["pluginauthor"] = plugin.PluginAuthor
			this.Data["pluginintro"] = plugin.PluginIntro
			this.Data["plugindetail"] = plugin.PluginDetail
			this.Data["plugintypeid"] = plugin.PluginTypeId
			this.Data["plugindocument"] = plugin.PluginDocument
			if plugin.PluginImage != "" {
				this.Data["pluginimage"] = utils.RawJS(plugin.PluginImage)
			}
			if plugin.PluginIsShare {
				this.Data["pluginisshare"] = "1"
			} else {
				this.Data["pluginisshare"] = "2"
			}
			tracelog.Info("查询插件信息", "Get", plugin.PluginName)

			if plugin.PluginIsShare == false {
				projectpluginmap, merr := projectPluginMapService.GetProjectPrivateMap(&this.Service, pluginid)
				if merr == nil {
					this.Data["projectid"] = projectpluginmap.ProjectId
				}
			}
		}
	}
	backUrl := this.Ctx.Request.Referer()
	if backUrl == "" {
		backUrl = "/shareplugin"
	}
	this.Data["backurl"] = backUrl

	this.Data["active"] = "menu2"
	this.TplNames = "submitplugin.html"
}

//提交插件
func (this *SubmitPluginController) Post() {
	var plugin models.Plugin
	pluginid := this.Input().Get("pluginid")
	if pluginid == "" {
		u, _ := uuid.NewV4()
		plugin.PluginId = u.String()
		plugin.CreateTime = time.Now()
	} else {
		plugin.PluginId = pluginid
		plugin, existErr := pluginService.GetPlugin(&this.Service, plugin.PluginId)
		if existErr != nil {
			plugin.CreateTime = time.Now()
		}
	}

	plugin.UpdateTime = time.Now()
	plugin.PluginName = this.Input().Get("pluginname")
	plugin.PluginAuthor = this.Input().Get("pluginauthor")
	plugin.PluginIntro = this.Input().Get("pluginintro")
	plugin.PluginDetail = this.Input().Get("plugindetail")
	plugin.PluginTypeId = this.Input().Get("plugintypeid")
	plugin.PluginIsShare = this.Input().Get("pluginisshare") == "1"

	tracelog.Info("保存插件图片", "Post", "开始保存")
	f, h, _ := this.GetFile("pluginimage")
	if f != nil {
		f.Close()
		tracelog.Info("保存插件图片", "Post", h.Filename)
		helper.CreatDirAll("static/plugin/"+plugin.PluginId+"/image", "/")
		this.SaveToFile("pluginimage", "./static/plugin/"+plugin.PluginId+"/image/"+h.Filename)
		plugin.PluginImage = "static/plugin/" + plugin.PluginId + "/image/" + h.Filename
	}

	tracelog.Info("保存插件图片", "Post", "结束保存")

	tracelog.Info("保存插件文档", "Post", "开始保存")
	df, dh, _ := this.GetFile("plugindocument")
	if df != nil {
		df.Close()
		tracelog.Info("保存插件文档", "Post", dh.Filename)
		helper.CreatDirAll("static/plugin/"+plugin.PluginId+"/doc", "/")
		this.SaveToFile("plugindocument", "./static/plugin/"+plugin.PluginId+"/doc/"+dh.Filename)
		plugin.PluginDocument = "static/plugin/" + plugin.PluginId + "/doc/" + dh.Filename
	}

	tracelog.Info("保存插件文档", "Post", "结束保存")

	tracelog.Info("保存插件信息", "Post", plugin.PluginName)

	pluginService.AddPlugin(&this.Service, &plugin)

	if plugin.PluginIsShare == false {
		var projectSelect = this.Input().Get("projectSelect")
		//先移除关系
		projectPluginMapService.DeleteProjectPrivateMap(&this.Service, plugin.PluginId)
		//添加一个新关系
		var projectPluginMap models.ProjectPluginMap
		projectPluginMap.ProjectId = projectSelect
		projectPluginMap.PluginId = plugin.PluginId
		projectPluginMapService.AddProjectPluginMap(&this.Service, &projectPluginMap)
	}

	this.Ctx.Redirect(302, "/submitplugin?pluginid="+plugin.PluginId+"&result=success")
}

//删除插件
func (this *SubmitPluginController) DeletePlugin() {
	pluginid := this.Input().Get("pluginid")
	pluginService.DeletePlugin(&this.Service, pluginid)
	this.Ctx.Redirect(302, "/shareplugin")
}
