package utils

import (
	"strings"

	"github.com/astaxie/beego"
	"github.com/goinggo/tracelog"
	"gopkg.in/mgo.v2"

	"OSGiServer/utils/helper"
)

func DbInit() (err error) {
	defer helper.CatchPanic(&err, "admin", "Service.Finish")

	tracelog.Trace("初始化", "DbInit", "开始初始化MongoDB数据库")

	isInit, initErr := beego.AppConfig.Bool("MongoDbInit")
	if !isInit || initErr != nil {
		tracelog.Info("初始化", "DbInit", "未配置初始化MongoDb数据库")
		return nil
	}

	var mongodbAddr = beego.AppConfig.String("MongoDbHosts")

	hosts := strings.Split(mongodbAddr, ",")

	session, err := mgo.Dial(hosts[0])
	if err != nil {
		tracelog.CompletedError(err, "DbInit", "连接MongoDb数据库出错")
	}

	defer session.Close()

	session.SetMode(mgo.Monotonic, true)

	isDrop, dropErr := beego.AppConfig.Bool("MongoDbDrop")
	if dropErr != nil {
		isDrop = true
	}

	if isDrop {
		err = session.DB("OSGi").DropDatabase()
		if err != nil {
			tracelog.CompletedError(err, "DbInit", "Drop数据库出错")
		}
	}

	c := session.DB("OSGi").C("Client")

	err = c.EnsureIndex(mgo.Index{
		Key:        []string{"clientmac"},
		Unique:     true,
		DropDups:   true,
		Background: true,
		Sparse:     true,
	})
	if err != nil {
		tracelog.Error(err, "Client", "创建表索引出错")
	}

	pt := session.DB("OSGi").C("PluginType")

	err = pt.EnsureIndex(mgo.Index{
		Key:        []string{"typeid"},
		Unique:     true,
		DropDups:   true,
		Background: true,
		Sparse:     true,
	})
	if err != nil {
		tracelog.Error(err, "PluginType", "创建表索引出错")
	}

	pj := session.DB("OSGi").C("Project")

	err = pj.EnsureIndex(mgo.Index{
		Key:        []string{"projectid"},
		Unique:     true,
		DropDups:   true,
		Background: true,
		Sparse:     true,
	})
	if err != nil {
		tracelog.Error(err, "Project", "创建表索引出错")
	}

	p := session.DB("OSGi").C("Plugin")

	err = p.EnsureIndex(mgo.Index{
		Key:        []string{"pluginid"},
		Unique:     true,
		DropDups:   true,
		Background: true,
		Sparse:     true,
	})
	if err != nil {
		tracelog.Error(err, "Plugin", "创建表索引出错")
	}

	pv := session.DB("OSGi").C("PluginVersion")

	err = pv.EnsureIndex(mgo.Index{
		Key:        []string{"pluginid,version"},
		Unique:     true,
		DropDups:   true,
		Background: true,
		Sparse:     true,
	})
	if err != nil {
		tracelog.Error(err, "PluginVersion", "创建表索引出错")
	}

	ppm := session.DB("OSGi").C("ProjectPluginMap")

	err = ppm.EnsureIndex(mgo.Index{
		Key:        []string{"pluginid,projectid"},
		Unique:     true,
		DropDups:   true,
		Background: true,
		Sparse:     true,
	})
	if err != nil {
		tracelog.Error(err, "ProjectPluginMap", "创建表索引出错")
	}

	tracelog.Trace("初始化", "DbInit", "结束初始化MongoDB数据库")

	return err
}
