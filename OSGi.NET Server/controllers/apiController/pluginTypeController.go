package apiController

import (
	"encoding/json"

	"github.com/goinggo/tracelog"

	"OSGiServer/controllers/baseController"
	"OSGiServer/models"
	"OSGiServer/services/pluginTypeService"
)

// 插件类型信息
type PluginTypeController struct {
	baseController.BaseController
}

// @Title 创建
// @Description 创建插件类型
// @Param	body	body	models.PluginType	true	"客户端信息JSON实体"
// @Success 200 {string} models.PluginType.typeid
// @Failure 403 body is empty
// @router / [post]
func (this *PluginTypeController) Post() {
	var oc models.PluginType
	err := json.Unmarshal(this.Ctx.Input.RequestBody, &oc)
	if err != nil {
		tracelog.Errorf(err, "PluginTypeController", "Post", "Body:%s", this.Ctx.Input.RequestBody)
		this.Data["json"] = err
	} else {
		typeid, dbErr := pluginTypeService.AddPluginType(&this.Service, &oc)
		if err != nil {
			tracelog.Error(err, "PluginTypeController", "Post-AddPluginType")
			this.Data["json"] = dbErr
		} else {
			this.Data["json"] = map[string]string{"PluginTypeId": typeid}
		}
	}
	this.ServeJson()
}

// @Title 查询
// @Description 查找指定插件类型信息
// @Param	typeid	path	string	true	"要查询的插件类型ID"
// @Success 200 {PluginType} models.PluginType
// @Failure 403 PluginTypeID is empty
// @router /:typeid [get]
func (this *PluginTypeController) Get() {
	typeid := this.Ctx.Input.Params[":typeid"]
	if typeid != "" {
		oc, err := pluginTypeService.GetPluginType(&this.Service, typeid)
		if err != nil {
			tracelog.Error(err, "PluginTypeController", "Get-GetPluginType")
			this.Data["json"] = err
		} else {
			this.Data["json"] = oc
		}
	} else {
		this.Data["json"] = "No TypeId"
	}
	this.ServeJson()
}

// @Title 查询所有
// @Description 获取插件类型列表
// @Success 200 {PluginType} models.PluginTypes
// @router / [get]
func (this *PluginTypeController) GetAll() {
	ocs, err := pluginTypeService.GetAllPluginTypes(&this.Service)
	if err != nil {
		tracelog.Error(err, "PluginTypeController", "GetAll-GetAllPluginTypes")
		this.Data["json"] = err
	} else {
		this.Data["json"] = ocs
	}
	this.ServeJson()
}

// @Title 删除
// @Description 删除指定插件类型
// @Param	typeid	path	string	true	"要删除的插件类型ID"
// @Success 200 {string} Delete Success
// @Failure 403 PluginTypeID is empty
// @router /:typeid [delete]
func (this *PluginTypeController) Delete() {
	typeid := this.Ctx.Input.Params[":typeid"]
	if typeid != "" {
		err := pluginTypeService.DeletePluginType(&this.Service, typeid)
		if err != nil {
			tracelog.Error(err, "PluginTypeController", "Delete-DeletePluginType")
			this.Data["json"] = err
		} else {
			this.Data["json"] = "Delete Success"
		}
	} else {
		this.Data["json"] = "No TypeId"
	}
	this.ServeJson()
}
