package main

import (
	_ "OSGiServer/docs"
	_ "OSGiServer/routers"

	"github.com/astaxie/beego"
	"github.com/goinggo/tracelog"

	"OSGiServer/utils"
	"OSGiServer/utils/mongo"
)

func main() {
	tracelog.Start(tracelog.LevelTrace)
	tracelog.Started(utils.MainGoRoutine, "Initializing Mongo")

	utils.DbInit()

	err := mongo.Startup(utils.MainGoRoutine)
	if err != nil {
		tracelog.CompletedError(err, utils.MainGoRoutine, "initApp")
	}

	if beego.RunMode == "dev" {
		beego.DirectoryIndex = true
		beego.StaticDir["/swagger"] = "swagger"
	}

	beego.Run()

	tracelog.Completed(utils.MainGoRoutine, "Website Shutdown")
	tracelog.Stop()
}
