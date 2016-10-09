package apiController

import (
	"encoding/json"

	"github.com/goinggo/tracelog"

	"OSGiServer/controllers/baseController"
	"OSGiServer/models"
	"OSGiServer/services/clientService"
)

// 客户端信息
type ClientController struct {
	baseController.BaseController
}

// @Title 创建
// @Description 创建客户端
// @Param	body	body	models.Client	true	"客户端信息JSON实体"
// @Success 200 {string} models.Client.ClientMac
// @Failure 403 body is empty
// @router / [post]
func (this *ClientController) Post() {
	var oc models.Client
	err := json.Unmarshal(this.Ctx.Input.RequestBody, &oc)
	if err != nil {
		tracelog.Errorf(err, "ClientController", "Post", "Body:%s", this.Ctx.Input.RequestBody)
		this.Data["json"] = err
	} else {
		clientMac, dbErr := clientService.AddClient(&this.Service, &oc)
		if err != nil {
			tracelog.Error(err, "ClientController", "Post-AddClient")
			this.Data["json"] = dbErr
		} else {
			this.Data["json"] = map[string]string{"ClientMac": clientMac}
		}
	}
	this.ServeJson()
}

// @Title 查询
// @Description 查找指定客户端信息
// @Param	clientMac	path	string	true	"要查询的客户端MAC地址"
// @Success 200 {Client} models.Client
// @Failure 403 ClientMac is empty
// @router /:clientMac [get]
func (this *ClientController) Get() {
	clientMac := this.Ctx.Input.Params[":clientMac"]
	if clientMac != "" {
		oc, err := clientService.GetClient(&this.Service, clientMac)
		if err != nil {
			tracelog.Error(err, "ClientController", "Get-GetClient")
			this.Data["json"] = err
		} else {
			this.Data["json"] = oc
		}
	} else {
		this.Data["json"] = "No clientMac"
	}
	this.ServeJson()
}

// @Title 查询所有
// @Description 获取客户信息列表
// @Success 200 {Client} models.Client
// @router / [get]
func (this *ClientController) GetAll() {
	ocs, err := clientService.GetAllClients(&this.Service)
	if err != nil {
		tracelog.Error(err, "ClientController", "GetAll-GetAllClients")
		this.Data["json"] = err
	} else {
		this.Data["json"] = ocs
	}
	this.ServeJson()
}

// @Title 更新状态
// @Description 更新指定客户端状态
// @Param	clientMac	path	string	true	"要更新的客户端MAC地址"
// @Param	status	path	string	true	"要更新的客户端状态"
// @Success 200 {string} Update Success
// @Failure 403 ClientMac is empty
// @router /:clientMac/:status [get]
func (this *ClientController) UpdateStatus() {
	clientMac := this.Ctx.Input.Params[":clientMac"]
	status := this.Ctx.Input.Params[":status"]
	if clientMac != "" {
		err := clientService.UpdateStatus(&this.Service, clientMac, status)
		if err != nil {
			tracelog.Error(err, "ClientController", "GetAll-UpdateStatus")
			this.Data["json"] = err
		} else {
			this.Data["json"] = "Update Success"
		}
	} else {
		this.Data["json"] = "No clientMac"
	}
	this.ServeJson()
}
