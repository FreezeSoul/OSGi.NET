package uiController

import (
	"strconv"

	"OSGiServer/controllers/baseController"
	"OSGiServer/models"
	"OSGiServer/services/pluginService"
	"OSGiServer/services/pluginTypeService"
	"OSGiServer/services/pluginVersionService"
	"OSGiServer/utils/helper"
)

type SharePluginController struct {
	baseController.BaseController
}

type UiPlugin struct {
	models.Plugin
	PluginTypeName string
	LastVersion    string
}

type UiPluginType struct {
	models.PluginType
	PluginCount int
}

func (this *SharePluginController) Get() {
	pagerowcount := 10
	typeId := this.Input().Get("typeid")
	pageindex, err := strconv.Atoi(this.Input().Get("page"))
	if err != nil || pageindex <= 0 {
		pageindex = 1
	}
	totalCount, _ := pluginService.GetPluginsCount(&this.Service, true, typeId)
	page := helper.NewPaging(pagerowcount, totalCount)
	page.SetTotalPage()

	prevpage, nextpage, pages := page.GetPageData(pageindex)
	this.Data["pages"] = pages
	this.Data["prevpage"] = prevpage
	this.Data["nextpage"] = nextpage
	this.Data["currentpage"] = strconv.Itoa(pageindex)

	this.Data["plugintypeid"] = typeId

	//获取所有插件类型
	types, _ := pluginTypeService.GetAllPluginTypes(&this.Service)
	uitypes := make([]UiPluginType, len(types))
	for i := 0; i < len(types); i++ {
		uitypes[i].PluginType = types[i]
		typeCount, _ := pluginService.GetPluginsCount(&this.Service, true, types[i].PluginTypeId)
		uitypes[i].PluginCount = typeCount
	}

	//获取当前页面所有插件
	plugins, _ := pluginService.GetPagingPlugins(&this.Service, true, typeId, pagerowcount*(pageindex-1), pagerowcount)
	uiplugins := make([]UiPlugin, len(plugins))
	for i := 0; i < len(plugins); i++ {
		uiplugins[i].Plugin = plugins[i]
		for _, ptype := range types {
			if uiplugins[i].PluginTypeId == ptype.PluginTypeId {
				uiplugins[i].PluginTypeName = ptype.PluginTypeName
			}
		}

		historyVersions, _ := pluginVersionService.GetPluginAllVersions(&this.Service, uiplugins[i].PluginId)

		if len(historyVersions) > 0 {
			uiplugins[i].LastVersion = historyVersions[len(historyVersions)-1].Version
		} else {
			uiplugins[i].LastVersion = "<无版本>"
		}

		if uiplugins[i].PluginImage == "" {
			uiplugins[i].PluginImage = "static/images/plugin.gif"
		}
	}

	this.Data["types"] = uitypes
	this.Data["plugins"] = uiplugins

	this.Data["active"] = "menu2"
	this.TplNames = "shareplugin.html"
}
