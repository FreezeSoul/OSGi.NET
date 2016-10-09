package projectPluginMapService

import (
	"github.com/goinggo/tracelog"
	"gopkg.in/mgo.v2"
	"gopkg.in/mgo.v2/bson"

	"OSGiServer/models"
	"OSGiServer/services"
)

func init() {
}

func AddProjectPluginMap(service *services.Service, projectPluginMap *models.ProjectPluginMap) (string, string, error) {

	tracelog.Startedf(service.UserID, "AddProjectPluginMap", "projectPluginMap%+v", &projectPluginMap)

	dberr := service.DBActionOSGi("ProjectPluginMap",
		func(collection *mgo.Collection) error {
			_, err1 := collection.Upsert(bson.M{"projectid": projectPluginMap.ProjectId, "pluginid": projectPluginMap.PluginId}, &projectPluginMap)
			return err1
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "AddProjectPluginMap")
		return "", "", dberr
	}

	tracelog.Completed(service.UserID, "AddProjectPluginMap")

	return projectPluginMap.ProjectId, projectPluginMap.PluginId, dberr
}

//获取项目共享插件引用
func GetProjectShareMap(service *services.Service, projectid string, pluginid string) (*models.ProjectPluginMap, error) {

	tracelog.Startedf(service.UserID, "GetProjectPluginMap", "projectid[%s]", projectid)

	result := models.ProjectPluginMap{}
	dberr := service.DBActionOSGi("ProjectPluginMap",
		func(collection *mgo.Collection) error {
			return collection.Find(bson.M{"projectid": projectid, "pluginid": pluginid}).One(&result)
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "GetProjectPluginMap")
		return nil, dberr
	}

	tracelog.Completedf(service.UserID, "GetProjectPluginMap", "projectPluginMap%+v", &result)

	return &result, dberr
}

func GetAllProjectPluginMaps(service *services.Service, projectid string) ([]models.ProjectPluginMap, error) {

	tracelog.Started(service.UserID, "GetAllProjectPluginMaps")

	var results []models.ProjectPluginMap
	dberr := service.DBActionOSGi("ProjectPluginMap",
		func(collection *mgo.Collection) error {
			return collection.Find(bson.M{"projectid": projectid}).All(&results)
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "GetAllProjectPluginMaps")
		return nil, dberr
	}

	tracelog.Completedf(service.UserID, "GetAllProjectPluginMaps", "projectPluginMap%+v", &results)

	return results, dberr
}

func GetAllProjectPluginMapCount(service *services.Service, projectid string) (int, error) {

	tracelog.Started(service.UserID, "GetAllProjectPluginMapCount")

	var count int
	dberr := service.DBActionOSGi("ProjectPluginMap",
		func(collection *mgo.Collection) error {
			var err error
			count, err = collection.Find(bson.M{"projectid": projectid}).Count()
			return err
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "GetAllProjectPluginMapCount")
		return 0, dberr
	}

	tracelog.Completedf(service.UserID, "GetAllProjectPluginMapCount", "plugincount[%s]", count)

	return count, dberr
}

//删除项目插件引用,共享及私有
func DeleteProjectMap(service *services.Service, projectid string, pluginid string) error {
	tracelog.Started(service.UserID, "DeleteProjectPluginMap")

	dberr := service.DBActionOSGi("ProjectPluginMap",
		func(collection *mgo.Collection) error {
			return collection.Remove(bson.M{"projectid": projectid, "pluginid": pluginid})
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "DeleteProjectPluginMap")
		return dberr
	}

	tracelog.Completed(service.UserID, "DeleteProjectPluginMap")

	return nil
}

//删除项目私有插件关系，注意，共有插件可能会被多个项目包含，此处不适合调用此方法
func DeleteProjectPrivateMap(service *services.Service, pluginid string) error {
	tracelog.Started(service.UserID, "DeleteProjectPluginMap")

	dberr := service.DBActionOSGi("ProjectPluginMap",
		func(collection *mgo.Collection) error {
			return collection.Remove(bson.M{"pluginid": pluginid})
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "DeleteProjectPluginMap")
		return dberr
	}

	tracelog.Completed(service.UserID, "DeleteProjectPluginMap")

	return nil
}

//获取项目私有插件关系，供插件修改时使用
func GetProjectPrivateMap(service *services.Service, pluginid string) (*models.ProjectPluginMap, error) {

	tracelog.Startedf(service.UserID, "GetProjectPluginMap", "pluginid[%s]", pluginid)

	result := models.ProjectPluginMap{}
	dberr := service.DBActionOSGi("ProjectPluginMap",
		func(collection *mgo.Collection) error {
			return collection.Find(bson.M{"pluginid": pluginid}).One(&result)
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "GetProjectPluginMap")
		return nil, dberr
	}

	tracelog.Completedf(service.UserID, "GetProjectPluginMap", "projectPluginMap%+v", &result)

	return &result, dberr
}

func UpdateManifest(service *services.Service, projectid string, pluginid string, manifest string) error {

	tracelog.Startedf(service.UserID, "UpdateStatus", "projectid[%s],pluginid[%s],manifest[%s]", projectid, pluginid, manifest)

	dberr := service.DBActionOSGi("ProjectPluginMap",
		func(collection *mgo.Collection) error {
			change := bson.M{"$set": bson.M{"pluginmanifest": manifest}}
			return collection.Update(bson.M{"projectid": projectid, "pluginid": pluginid}, change)
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "UpdateManifest")
		return dberr
	}

	tracelog.Completed(service.UserID, "UpdateManifest")
	return nil
}
