package uiController

import (
	"OSGiServer/controllers/baseController"
	"OSGiServer/models"
	"OSGiServer/services/projectPluginMapService"
	"OSGiServer/services/projectService"
)

type ProjectPluginController struct {
	baseController.BaseController
}

type UiProject struct {
	models.Project
	PluginCount int
}

func (this *ProjectPluginController) Get() {
	projects, _ := projectService.GetAllProjects(&this.Service)

	uiprojects := make([]UiProject, len(projects))

	for i := 0; i < len(projects); i++ {
		uiprojects[i].Project = projects[i]
		projectid := projects[i].ProjectId
		count, err := projectPluginMapService.GetAllProjectPluginMapCount(&this.Service, projectid)
		if err == nil {
			uiprojects[i].PluginCount = count
		} else {
			uiprojects[i].PluginCount = 0
		}
	}

	this.Data["projects"] = uiprojects

	this.Data["active"] = "menu2"
	this.TplNames = "projectplugin.html"
}
