package pluginService

import (
	"github.com/goinggo/tracelog"
	"gopkg.in/mgo.v2"
	"gopkg.in/mgo.v2/bson"

	"OSGiServer/models"
	"OSGiServer/services"
)

func init() {
}

func AddPlugin(service *services.Service, plugin *models.Plugin) (string, error) {

	tracelog.Startedf(service.UserID, "AddPlugin", "plugin%+v", &plugin)

	dberr := service.DBActionOSGi("Plugin",
		func(collection *mgo.Collection) error {
			_, err1 := collection.Upsert(bson.M{"pluginid": plugin.PluginId}, &plugin)
			return err1
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "AddPlugin")
		return "", dberr
	}

	tracelog.Completed(service.UserID, "AddPlugin")

	return plugin.PluginId, dberr
}

func GetPlugin(service *services.Service, pluginid string) (*models.Plugin, error) {

	tracelog.Startedf(service.UserID, "GetPlugin", "pluginid[%s]", pluginid)

	result := models.Plugin{}
	dberr := service.DBActionOSGi("Plugin",
		func(collection *mgo.Collection) error {
			return collection.Find(bson.M{"pluginid": pluginid}).One(&result)
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "GetPlugin")
		return nil, dberr
	}

	tracelog.Completedf(service.UserID, "GetPlugin", "plugin%+v", &result)

	return &result, dberr
}

func GetAllPlugins(service *services.Service) ([]models.Plugin, error) {

	tracelog.Started(service.UserID, "GetAllPlugins")

	var results []models.Plugin
	dberr := service.DBActionOSGi("Plugin",
		func(collection *mgo.Collection) error {
			return collection.Find(bson.M{}).All(&results)
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "GetAllPlugins")
		return nil, dberr
	}

	tracelog.Completedf(service.UserID, "GetAllPlugins", "plugin%+v", &results)

	return results, dberr
}

func GetPagingPlugins(service *services.Service, isshare bool, typeid string, skip int, take int) ([]models.Plugin, error) {
	tracelog.Started(service.UserID, "GetPagingPllugins")

	var results []models.Plugin
	dberr := service.DBActionOSGi("Plugin",
		func(collection *mgo.Collection) error {
			var q interface{}
			if typeid == "" {
				q = bson.M{"pluginisshare": isshare}
			} else {
				q = bson.M{"plugintypeid": typeid, "pluginisshare": isshare}
			}
			return collection.Find(q).Skip(skip).Sort("-updatetime").Limit(take).All(&results)
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "GetPagingPllugins")
		return nil, dberr
	}

	tracelog.Completedf(service.UserID, "GetPagingPllugins", "plugin%+v", &results)

	return results, dberr
}

func GetPluginsCount(service *services.Service, isshare bool, typeid string) (int, error) {
	tracelog.Started(service.UserID, "GetPluginsCount")

	var count int
	dberr := service.DBActionOSGi("Plugin",
		func(collection *mgo.Collection) error {
			var err error
			var q interface{}
			if typeid == "" {
				q = bson.M{"pluginisshare": isshare}
			} else {
				q = bson.M{"plugintypeid": typeid, "pluginisshare": isshare}
			}
			count, err = collection.Find(q).Count()
			return err
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "GetPluginsCount")
		return 0, dberr
	}

	tracelog.Completedf(service.UserID, "GetPluginsCount", "plugincount[%s]", count)

	return count, dberr
}

func DeletePlugin(service *services.Service, pluginid string) error {
	tracelog.Started(service.UserID, "DeletePlugin")

	dberr := service.DBActionOSGi("Plugin",
		func(collection *mgo.Collection) error {
			return collection.Remove(bson.M{"pluginid": pluginid})
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "DeletePlugin")
		return dberr
	}

	tracelog.Completed(service.UserID, "DeletePlugin")

	return nil
}
