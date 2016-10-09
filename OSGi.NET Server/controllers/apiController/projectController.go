package apiController

import (
	"encoding/json"

	"github.com/goinggo/tracelog"

	"OSGiServer/controllers/baseController"
	"OSGiServer/models"
	"OSGiServer/services/projectService"
)

// 项目信息
type ProjectController struct {
	baseController.BaseController
}

// @Title 创建
// @Description 创建项目
// @Param	body	body	models.Project	true	"客户端信息JSON实体"
// @Success 200 {string} models.Project.projectid
// @Failure 403 body is empty
// @router / [post]
func (this *ProjectController) Post() {
	var oc models.Project
	err := json.Unmarshal(this.Ctx.Input.RequestBody, &oc)
	if err != nil {
		tracelog.Errorf(err, "ProjectController", "Post", "Body:%s", this.Ctx.Input.RequestBody)
		this.Data["json"] = err
	} else {
		projectid, dbErr := projectService.AddProject(&this.Service, &oc)
		if err != nil {
			tracelog.Error(err, "ProjectController", "Post-AddProject")
			this.Data["json"] = dbErr
		} else {
			this.Data["json"] = map[string]string{"ProjectId": projectid}
		}
	}
	this.ServeJson()
}

// @Title 查询
// @Description 查找指定项目信息
// @Param	projectid	path	string	true	"要查询的项目ID"
// @Success 200 {Project} models.Project
// @Failure 403 ProjectID is empty
// @router /:projectid [get]
func (this *ProjectController) Get() {
	projectid := this.Ctx.Input.Params[":projectid"]
	if projectid != "" {
		oc, err := projectService.GetProject(&this.Service, projectid)
		if err != nil {
			tracelog.Error(err, "ProjectController", "Get-GetProject")
			this.Data["json"] = err
		} else {
			this.Data["json"] = oc
		}
	} else {
		this.Data["json"] = "No ProjectId"
	}
	this.ServeJson()
}

// @Title 查询所有
// @Description 获取项目列表
// @Success 200 {Project} models.Projects
// @router / [get]
func (this *ProjectController) GetAll() {
	ocs, err := projectService.GetAllProjects(&this.Service)
	if err != nil {
		tracelog.Error(err, "ProjectController", "GetAll-GetAllProjects")
		this.Data["json"] = err
	} else {
		this.Data["json"] = ocs
	}
	this.ServeJson()
}

// @Title 删除
// @Description 删除指定项目
// @Param	projectid	path	string	true	"要删除的项目ID"
// @Success 200 {string} Delete Success
// @Failure 403 ProjectID is empty
// @router /:projectid [delete]
func (this *ProjectController) Delete() {
	projectid := this.Ctx.Input.Params[":projectid"]
	if projectid != "" {
		err := projectService.DeleteProject(&this.Service, projectid)
		if err != nil {
			tracelog.Error(err, "ProjectController", "Delete-DeleteProject")
			this.Data["json"] = err
		} else {
			this.Data["json"] = "Delete Success"
		}
	} else {
		this.Data["json"] = "No ProjectId"
	}
	this.ServeJson()
}
