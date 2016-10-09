package routers

import (
	"github.com/astaxie/beego"
)

func init() {
	
	beego.GlobalControllerRouter["OSGiServer/controllers/apiController:PluginController"] = append(beego.GlobalControllerRouter["OSGiServer/controllers/apiController:PluginController"],
		beego.ControllerComments{
			"Post",
			`/`,
			[]string{"post"},
			nil})

	beego.GlobalControllerRouter["OSGiServer/controllers/apiController:PluginController"] = append(beego.GlobalControllerRouter["OSGiServer/controllers/apiController:PluginController"],
		beego.ControllerComments{
			"Get",
			`/:pluginid`,
			[]string{"get"},
			nil})

	beego.GlobalControllerRouter["OSGiServer/controllers/apiController:PluginController"] = append(beego.GlobalControllerRouter["OSGiServer/controllers/apiController:PluginController"],
		beego.ControllerComments{
			"GetAll",
			`/`,
			[]string{"get"},
			nil})

	beego.GlobalControllerRouter["OSGiServer/controllers/apiController:PluginController"] = append(beego.GlobalControllerRouter["OSGiServer/controllers/apiController:PluginController"],
		beego.ControllerComments{
			"Delete",
			`/:pluginid`,
			[]string{"delete"},
			nil})

	beego.GlobalControllerRouter["OSGiServer/controllers/apiController:PluginTypeController"] = append(beego.GlobalControllerRouter["OSGiServer/controllers/apiController:PluginTypeController"],
		beego.ControllerComments{
			"Post",
			`/`,
			[]string{"post"},
			nil})

	beego.GlobalControllerRouter["OSGiServer/controllers/apiController:PluginTypeController"] = append(beego.GlobalControllerRouter["OSGiServer/controllers/apiController:PluginTypeController"],
		beego.ControllerComments{
			"Get",
			`/:typeid`,
			[]string{"get"},
			nil})

	beego.GlobalControllerRouter["OSGiServer/controllers/apiController:PluginTypeController"] = append(beego.GlobalControllerRouter["OSGiServer/controllers/apiController:PluginTypeController"],
		beego.ControllerComments{
			"GetAll",
			`/`,
			[]string{"get"},
			nil})

	beego.GlobalControllerRouter["OSGiServer/controllers/apiController:PluginTypeController"] = append(beego.GlobalControllerRouter["OSGiServer/controllers/apiController:PluginTypeController"],
		beego.ControllerComments{
			"Delete",
			`/:typeid`,
			[]string{"delete"},
			nil})

	beego.GlobalControllerRouter["OSGiServer/controllers/apiController:ProjectController"] = append(beego.GlobalControllerRouter["OSGiServer/controllers/apiController:ProjectController"],
		beego.ControllerComments{
			"Post",
			`/`,
			[]string{"post"},
			nil})

	beego.GlobalControllerRouter["OSGiServer/controllers/apiController:ProjectController"] = append(beego.GlobalControllerRouter["OSGiServer/controllers/apiController:ProjectController"],
		beego.ControllerComments{
			"Get",
			`/:projectid`,
			[]string{"get"},
			nil})

	beego.GlobalControllerRouter["OSGiServer/controllers/apiController:ProjectController"] = append(beego.GlobalControllerRouter["OSGiServer/controllers/apiController:ProjectController"],
		beego.ControllerComments{
			"GetAll",
			`/`,
			[]string{"get"},
			nil})

	beego.GlobalControllerRouter["OSGiServer/controllers/apiController:ProjectController"] = append(beego.GlobalControllerRouter["OSGiServer/controllers/apiController:ProjectController"],
		beego.ControllerComments{
			"Delete",
			`/:projectid`,
			[]string{"delete"},
			nil})

	beego.GlobalControllerRouter["OSGiServer/controllers/apiController:ClientController"] = append(beego.GlobalControllerRouter["OSGiServer/controllers/apiController:ClientController"],
		beego.ControllerComments{
			"Post",
			`/`,
			[]string{"post"},
			nil})

	beego.GlobalControllerRouter["OSGiServer/controllers/apiController:ClientController"] = append(beego.GlobalControllerRouter["OSGiServer/controllers/apiController:ClientController"],
		beego.ControllerComments{
			"Get",
			`/:clientMac`,
			[]string{"get"},
			nil})

	beego.GlobalControllerRouter["OSGiServer/controllers/apiController:ClientController"] = append(beego.GlobalControllerRouter["OSGiServer/controllers/apiController:ClientController"],
		beego.ControllerComments{
			"GetAll",
			`/`,
			[]string{"get"},
			nil})

	beego.GlobalControllerRouter["OSGiServer/controllers/apiController:ClientController"] = append(beego.GlobalControllerRouter["OSGiServer/controllers/apiController:ClientController"],
		beego.ControllerComments{
			"UpdateStatus",
			`/:clientMac/:status`,
			[]string{"get"},
			nil})

}
