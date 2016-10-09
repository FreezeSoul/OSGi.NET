package uiController

import (
	"OSGiServer/controllers/baseController"
	"OSGiServer/services/clientService"
)

type ManagerController struct {
	baseController.BaseController
}

func (this *ManagerController) Get() {
	clients, _ := clientService.GetAllClients(&this.Service)
	this.Data["clients"] = clients
	this.Data["active"] = "menu1"
	this.TplNames = "manager.html"
}
