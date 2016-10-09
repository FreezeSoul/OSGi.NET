package routers

import (
	"github.com/astaxie/beego"
	"github.com/astaxie/beego/context"

	"OSGiServer/controllers/apiController"
	"OSGiServer/controllers/uiController"
)

func init() {
	beego.Router("/", &uiController.MainController{})
	beego.Router("/manager", &uiController.ManagerController{})
	beego.Router("/telnet", &uiController.TelnetController{})

	beego.Router("/shareplugin", &uiController.SharePluginController{})
	beego.Router("/submitplugin", &uiController.SubmitPluginController{})
	beego.Router("/projectplugin", &uiController.ProjectPluginController{})
	beego.Router("/plugindetail", &uiController.PluginDetailController{})
	beego.Router("/publishplugin", &uiController.PublishPluginController{})
	beego.Router("/pluginversion", &uiController.PluginVersionController{})
	beego.Router("/projectdetail", &uiController.ProjectDetailController{})

	beego.Router("/deleteplugin", &uiController.SubmitPluginController{}, "get:DeletePlugin")
	beego.Router("/deleteversion", &uiController.PublishPluginController{}, "get:DeleteVersion")
	beego.Router("/updatemanifest", &uiController.ProjectDetailController{}, "post:UpdateManifest")
	beego.Router("/deletepluginmap", &uiController.ProjectDetailController{}, "get:DeletePluginMap")

	beego.InsertFilter("*", beego.BeforeRouter, func(ctx *context.Context) {
		ctx.Output.Header("Access-Control-Allow-Origin", "*")
	})

	ns := beego.NewNamespace("/v1",
		beego.NSNamespace("/client",
			beego.NSInclude(
				&apiController.ClientController{},
			),
		),
		beego.NSNamespace("/plugintype",
			beego.NSInclude(
				&apiController.PluginTypeController{},
			),
		),
		beego.NSNamespace("/project",
			beego.NSInclude(
				&apiController.ProjectController{},
			),
		),
		beego.NSNamespace("/plugin",
			beego.NSInclude(
				&apiController.PluginController{},
			),
		),
	)
	beego.AddNamespace(ns)

}
