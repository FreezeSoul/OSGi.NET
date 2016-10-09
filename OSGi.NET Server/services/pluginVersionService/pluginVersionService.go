package pluginVersionService

import (
	"github.com/goinggo/tracelog"
	"gopkg.in/mgo.v2"
	"gopkg.in/mgo.v2/bson"

	"OSGiServer/models"
	"OSGiServer/services"
)

func init() {
}

func AddPluginVersion(service *services.Service, pluginVersion *models.PluginVersion) (string, string, error) {

	tracelog.Startedf(service.UserID, "AddPluginVersion", "pluginVersion%+v", &pluginVersion)

	dberr := service.DBActionOSGi("PluginVersion",
		func(collection *mgo.Collection) error {
			_, err1 := collection.Upsert(bson.M{"pluginid": pluginVersion.PluginId, "version": pluginVersion.Version}, &pluginVersion)
			return err1
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "AddPluginVersion")
		return "", "", dberr
	}

	tracelog.Completed(service.UserID, "AddPluginVersion")

	return pluginVersion.PluginId, pluginVersion.Version, dberr
}

func GetPluginVersion(service *services.Service, pluginid string, version string) (*models.PluginVersion, error) {

	tracelog.Startedf(service.UserID, "GetPluginVersion", "pluginid[%s]", pluginid)

	result := models.PluginVersion{}
	dberr := service.DBActionOSGi("PluginVersion",
		func(collection *mgo.Collection) error {
			return collection.Find(bson.M{"pluginid": pluginid, "version": version}).One(&result)
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "GetPluginVersion")
		return nil, dberr
	}

	tracelog.Completedf(service.UserID, "GetPluginVersion", "pluginVersion%+v", &result)

	return &result, dberr
}

func GetPluginAllVersions(service *services.Service, pluginid string) ([]models.PluginVersion, error) {

	tracelog.Started(service.UserID, "GetPluginAllVersions")

	var results []models.PluginVersion
	dberr := service.DBActionOSGi("PluginVersion",
		func(collection *mgo.Collection) error {
			return collection.Find(bson.M{"pluginid": pluginid}).All(&results)
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "GetPluginAllVersions")
		return nil, dberr
	}

	tracelog.Completedf(service.UserID, "GetPluginAllVersions", "pluginVersion%+v", &results)

	return results, dberr
}

func DeletePluginVersion(service *services.Service, pluginid string, version string) error {
	tracelog.Started(service.UserID, "DeletePluginVersion")

	dberr := service.DBActionOSGi("PluginVersion",
		func(collection *mgo.Collection) error {
			return collection.Remove(bson.M{"pluginid": pluginid, "version": version})
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "DeletePluginVersion")
		return dberr
	}

	tracelog.Completed(service.UserID, "DeletePluginVersion")

	return nil
}

func DeletePluginAllVersion(service *services.Service, pluginid string) error {
	tracelog.Started(service.UserID, "DeletePluginAllVersion")

	dberr := service.DBActionOSGi("PluginVersion",
		func(collection *mgo.Collection) error {
			return collection.Remove(bson.M{"pluginid": pluginid})
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "DeletePluginAllVersion")
		return dberr
	}

	tracelog.Completed(service.UserID, "DeletePluginAllVersion")

	return nil
}
