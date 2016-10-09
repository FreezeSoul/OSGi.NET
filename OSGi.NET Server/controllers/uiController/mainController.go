package uiController

import (
	"OSGiServer/controllers/baseController"
)

type MainController struct {
	baseController.BaseController
}

func (this *MainController) Get() {
	this.Data["Website"] = "OSGi组件仓库"
	this.Data["Email"] = "freezesoul@gmail.com"
	this.TplNames = "index.html"
}
