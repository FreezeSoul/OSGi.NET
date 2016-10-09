package projectService

import (
	"github.com/goinggo/tracelog"
	"gopkg.in/mgo.v2"
	"gopkg.in/mgo.v2/bson"

	"OSGiServer/models"
	"OSGiServer/services"
)

func init() {
}

func AddProject(service *services.Service, project *models.Project) (string, error) {

	tracelog.Startedf(service.UserID, "AddProject", "project%+v", &project)

	dberr := service.DBActionOSGi("Project",
		func(collection *mgo.Collection) error {
			_, err1 := collection.Upsert(bson.M{"projectid": project.ProjectId}, &project)
			return err1
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "AddProject")
		return "", dberr
	}

	tracelog.Completed(service.UserID, "AddProject")

	return project.ProjectId, dberr
}

func GetProject(service *services.Service, projectid string) (*models.Project, error) {

	tracelog.Startedf(service.UserID, "GetProject", "projectid[%s]", projectid)

	result := models.Project{}
	dberr := service.DBActionOSGi("Project",
		func(collection *mgo.Collection) error {
			return collection.Find(bson.M{"projectid": projectid}).One(&result)
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "GetProject")
		return nil, dberr
	}

	tracelog.Completedf(service.UserID, "GetProject", "project%+v", &result)

	return &result, dberr
}

func GetAllProjects(service *services.Service) ([]models.Project, error) {

	tracelog.Started(service.UserID, "GetAllProjects")

	var results []models.Project
	dberr := service.DBActionOSGi("Project",
		func(collection *mgo.Collection) error {
			return collection.Find(bson.M{}).All(&results)
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "GetAllProjects")
		return nil, dberr
	}

	tracelog.Completedf(service.UserID, "GetAllProjects", "project%+v", &results)

	return results, dberr
}

func DeleteProject(service *services.Service, projectid string) error {
	tracelog.Started(service.UserID, "DeleteProject")

	dberr := service.DBActionOSGi("Project",
		func(collection *mgo.Collection) error {
			return collection.Remove(bson.M{"projectid": projectid})
		})

	if dberr != nil {
		tracelog.CompletedError(dberr, service.UserID, "DeleteProject")
		return dberr
	}

	tracelog.Completed(service.UserID, "DeleteProject")

	return nil
}
