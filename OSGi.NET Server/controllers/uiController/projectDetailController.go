package uiController

import (
	"strconv"
	"strings"

	"github.com/goinggo/tracelog"

	"OSGiServer/controllers/baseController"
	"OSGiServer/models"
	"OSGiServer/services/pluginService"
	"OSGiServer/services/pluginTypeService"
	"OSGiServer/services/pluginVersionService"
	"OSGiServer/services/projectPluginMapService"
	"OSGiServer/services/projectService"
)

type ProjectDetailController struct {
	baseController.BaseController
}

type UiProjectPluginType struct {
	models.PluginType
	PluginCount int
	Plugins     []UiProjectPlugin
}

type UiProjectPlugin struct {
	models.Plugin
	PluginManifest string
	LastVersion    string
}

func (this *ProjectDetailController) Get() {
	projectid := this.Input().Get("projectid")
	this.Data["projectid"] = projectid

	projects, _ := projectService.GetAllProjects(&this.Service)

	uiprojects := make([]UiProject, len(projects))

	for i := 0; i < len(projects); i++ {
		uiprojects[i].Project = projects[i]
		theprojectid := projects[i].ProjectId
		count, err := projectPluginMapService.GetAllProjectPluginMapCount(&this.Service, theprojectid)
		if err == nil {
			uiprojects[i].PluginCount = count
		} else {
			uiprojects[i].PluginCount = 0
		}
	}

	this.Data["projects"] = uiprojects

	project, perr := projectService.GetProject(&this.Service, projectid)
	if perr == nil {
		this.Data["project"] = project
	}

	//获取所有的关联插件
	ppmaps, merr := projectPluginMapService.GetAllProjectPluginMaps(&this.Service, projectid)
	uiallplugins := make([]UiProjectPlugin, len(ppmaps))
	if merr == nil {
		for i := 0; i < len(ppmaps); i++ {
			thepluginid := ppmaps[i].PluginId

			//关联的插件信息
			plugin, pgerr := pluginService.GetPlugin(&this.Service, thepluginid)
			if pgerr != nil {
				continue
			}

			uiallplugins[i].Plugin = *plugin

			//关联版本的配置清单信息
			uiallplugins[i].PluginManifest = ppmaps[i].PluginManifest

			//最新版本
			historyVersions, _ := pluginVersionService.GetPluginAllVersions(&this.Service, thepluginid)
			tracelog.Info("项目详细信息查询", "Get", plugin.PluginName+" Has Versions:"+strconv.Itoa(len(historyVersions)))

			if len(historyVersions) > 0 {
				uiallplugins[i].LastVersion = historyVersions[len(historyVersions)-1].Version
			} else {
				uiallplugins[i].LastVersion = "<无版本>"
			}

		}
	}

	//获取所有插件类型
	types, _ := pluginTypeService.GetAllPluginTypes(&this.Service)
	uitypes := make([]UiProjectPluginType, len(types))
	for i := 0; i < len(types); i++ {
		uitypes[i].PluginType = types[i]
		typeCount, _ := pluginService.GetPluginsCount(&this.Service, true, types[i].PluginTypeId)
		uitypes[i].PluginCount = typeCount
		setPluginTypeOfProjectPlugins(&uitypes[i], uiallplugins)
	}

	this.Data["plugintypes"] = uitypes

	this.Data["active"] = "menu2"
	this.TplNames = "projectdetail.html"
}

func setPluginTypeOfProjectPlugins(plugintype *UiProjectPluginType, plugins []UiProjectPlugin) {
	length := 0
	for i := 0; i < len(plugins); i++ {
		if plugins[i].PluginTypeId == plugintype.PluginTypeId {
			length = length + 1
		}
	}
	plugintype.Plugins = make([]UiProjectPlugin, length, 5)
	plugintype.PluginCount = length
	index := 0
	for i := 0; i < len(plugins); i++ {
		if plugins[i].PluginTypeId == plugintype.PluginTypeId {
			if plugins[i].PluginManifest == "" {
				plugins[i].PluginManifest = "无"
			}
			plugintype.Plugins[index] = plugins[i]
			index = index + 1
		}
	}
}

//更新项目插件清单信息
func (this *ProjectDetailController) UpdateManifest() {
	projectid := this.Input().Get("projectid")
	pluginid := this.Input().Get("pluginid")
	value := this.Input().Get("value")

	projectPluginMapService.UpdateManifest(&this.Service, projectid, pluginid, value)

	tracelog.Info("更新项目插件清单信息", "UpdateManifest", value)
	this.Ctx.WriteString("Finish")
}

//移除插件映射信息
func (this *ProjectDetailController) DeletePluginMap() {
	projectid := this.Input().Get("projectid")
	pluginid := this.Input().Get("pluginid")
	isshare := this.Input().Get("isshare")
	//1.删除关系
	projectPluginMapService.DeleteProjectMap(&this.Service, projectid, pluginid)

	if strings.ToLower(isshare) == "false" {
		//2.删除插件
		pluginService.DeletePlugin(&this.Service, pluginid)

		//3.删除版本信息
		pluginVersionService.DeletePluginAllVersion(&this.Service, pluginid)
	}

	this.Ctx.Redirect(302, "/projectdetail?projectid="+projectid+"&result=success")

}
