package apiController

import (
	"encoding/json"

	"github.com/goinggo/tracelog"

	"OSGiServer/controllers/baseController"
	"OSGiServer/models"
	"OSGiServer/services/pluginService"
)

// 插件信息
type PluginController struct {
	baseController.BaseController
}

// @Title 创建
// @Description 创建插件
// @Param	body	body	models.Plugin	true	"客户端信息JSON实体"
// @Success 200 {string} models.Plugin.pluginid
// @Failure 403 body is empty
// @router / [post]
func (this *PluginController) Post() {
	var oc models.Plugin
	err := json.Unmarshal(this.Ctx.Input.RequestBody, &oc)
	if err != nil {
		tracelog.Errorf(err, "PluginController", "Post", "Body:%s", this.Ctx.Input.RequestBody)
		this.Data["json"] = err
	} else {
		pluginid, dbErr := pluginService.AddPlugin(&this.Service, &oc)
		if err != nil {
			tracelog.Error(err, "PluginController", "Post-AddPlugin")
			this.Data["json"] = dbErr
		} else {
			this.Data["json"] = map[string]string{"PluginId": pluginid}
		}
	}
	this.ServeJson()
}

// @Title 查询
// @Description 查找指定插件信息
// @Param	pluginid	path	string	true	"要查询的插件ID"
// @Success 200 {Plugin} models.Plugin
// @Failure 403 PluginID is empty
// @router /:pluginid [get]
func (this *PluginController) Get() {
	pluginid := this.Ctx.Input.Params[":pluginid"]
	if pluginid != "" {
		oc, err := pluginService.GetPlugin(&this.Service, pluginid)
		if err != nil {
			tracelog.Error(err, "PluginController", "Get-GetPlugin")
			this.Data["json"] = err
		} else {
			this.Data["json"] = oc
		}
	} else {
		this.Data["json"] = "No PluginID"
	}
	this.ServeJson()
}

// @Title 查询所有
// @Description 获取插件列表
// @Success 200 {Plugin} models.Plugin
// @router / [get]
func (this *PluginController) GetAll() {
	ocs, err := pluginService.GetAllPlugins(&this.Service)
	if err != nil {
		tracelog.Error(err, "PluginController", "GetAll-GetAllPlugins")
		this.Data["json"] = err
	} else {
		this.Data["json"] = ocs
	}
	this.ServeJson()
}

// @Title 删除
// @Description 删除指定插件
// @Param	pluginid	path	string	true	"要删除的插件ID"
// @Success 200 {string} Delete Success
// @Failure 403 PluginID is empty
// @router /:pluginid [delete]
func (this *PluginController) Delete() {
	pluginid := this.Ctx.Input.Params[":pluginid"]
	if pluginid != "" {
		err := pluginService.DeletePlugin(&this.Service, pluginid)
		if err != nil {
			tracelog.Error(err, "PluginController", "Delete-DeletePlugin")
			this.Data["json"] = err
		} else {
			this.Data["json"] = "Delete Success"
		}
	} else {
		this.Data["json"] = "No PluginID"
	}
	this.ServeJson()
}
