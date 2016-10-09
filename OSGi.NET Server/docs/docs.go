package docs

import (
	"encoding/json"
	"strings"

	"github.com/astaxie/beego"
	"github.com/astaxie/beego/swagger"
)

var rootinfo string = `{"apiVersion":"","swaggerVersion":"1.2","apis":[{"path":"/client","description":"客户端信息\n"},{"path":"/plugintype","description":"插件类型信息\n"},{"path":"/project","description":"项目信息\n"},{"path":"/plugin","description":"插件信息\n"}],"info":{}}`
var subapi string = `{"/client":{"apiVersion":"","swaggerVersion":"1.2","basePath":"","resourcePath":"/client","produces":["application/json","application/xml","text/plain","text/html"],"apis":[{"path":"/","description":"","operations":[{"httpMethod":"POST","nickname":"创建","type":"","summary":"创建客户端","parameters":[{"paramType":"body","name":"body","description":"\"客户端信息JSON实体\"","dataType":"Client","type":"","format":"","allowMultiple":false,"required":true,"minimum":0,"maximum":0}],"responseMessages":[{"code":200,"message":"models.Client.ClientMac","responseModel":""},{"code":403,"message":"body is empty","responseModel":""}]}]},{"path":"/:clientMac","description":"","operations":[{"httpMethod":"GET","nickname":"查询","type":"","summary":"查找指定客户端信息","parameters":[{"paramType":"path","name":"clientMac","description":"\"要查询的客户端MAC地址\"","dataType":"string","type":"","format":"","allowMultiple":false,"required":true,"minimum":0,"maximum":0}],"responseMessages":[{"code":200,"message":"models.Client","responseModel":""},{"code":403,"message":"ClientMac is empty","responseModel":""}]}]},{"path":"/","description":"","operations":[{"httpMethod":"GET","nickname":"查询所有","type":"","summary":"获取客户信息列表","responseMessages":[{"code":200,"message":"models.Client","responseModel":""}]}]},{"path":"/:clientMac/:status","description":"","operations":[{"httpMethod":"GET","nickname":"更新状态","type":"","summary":"更新指定客户端状态","parameters":[{"paramType":"path","name":"clientMac","description":"\"要更新的客户端MAC地址\"","dataType":"string","type":"","format":"","allowMultiple":false,"required":true,"minimum":0,"maximum":0},{"paramType":"path","name":"status","description":"\"要更新的客户端状态\"","dataType":"string","type":"","format":"","allowMultiple":false,"required":true,"minimum":0,"maximum":0}],"responseMessages":[{"code":200,"message":"Success","responseModel":""},{"code":403,"message":"ClientMac is empty","responseModel":""}]}]}]},"/plugin":{"apiVersion":"","swaggerVersion":"1.2","basePath":"","resourcePath":"/plugin","produces":["application/json","application/xml","text/plain","text/html"],"apis":[{"path":"/","description":"","operations":[{"httpMethod":"POST","nickname":"创建","type":"","summary":"创建插件","parameters":[{"paramType":"body","name":"body","description":"\"客户端信息JSON实体\"","dataType":"Plugin","type":"","format":"","allowMultiple":false,"required":true,"minimum":0,"maximum":0}],"responseMessages":[{"code":200,"message":"models.Plugin.pluginid","responseModel":""},{"code":403,"message":"body is empty","responseModel":""}]}]},{"path":"/:pluginid","description":"","operations":[{"httpMethod":"GET","nickname":"查询","type":"","summary":"查找指定插件信息","parameters":[{"paramType":"path","name":"pluginid","description":"\"要查询的插件ID\"","dataType":"string","type":"","format":"","allowMultiple":false,"required":true,"minimum":0,"maximum":0}],"responseMessages":[{"code":200,"message":"models.Plugin","responseModel":""},{"code":403,"message":"PluginID is empty","responseModel":""}]}]},{"path":"/","description":"","operations":[{"httpMethod":"GET","nickname":"查询所有","type":"","summary":"获取插件列表","responseMessages":[{"code":200,"message":"models.Plugin","responseModel":""}]}]},{"path":"/:pluginid","description":"","operations":[{"httpMethod":"DELETE","nickname":"删除","type":"","summary":"删除指定插件","parameters":[{"paramType":"path","name":"pluginid","description":"\"要删除的插件ID\"","dataType":"string","type":"","format":"","allowMultiple":false,"required":true,"minimum":0,"maximum":0}],"responseMessages":[{"code":200,"message":"Success","responseModel":""},{"code":403,"message":"PluginID is empty","responseModel":""}]}]}]},"/plugintype":{"apiVersion":"","swaggerVersion":"1.2","basePath":"","resourcePath":"/plugintype","produces":["application/json","application/xml","text/plain","text/html"],"apis":[{"path":"/","description":"","operations":[{"httpMethod":"POST","nickname":"创建","type":"","summary":"创建插件类型","parameters":[{"paramType":"body","name":"body","description":"\"客户端信息JSON实体\"","dataType":"PluginType","type":"","format":"","allowMultiple":false,"required":true,"minimum":0,"maximum":0}],"responseMessages":[{"code":200,"message":"models.PluginType.typeid","responseModel":""},{"code":403,"message":"body is empty","responseModel":""}]}]},{"path":"/:typeid","description":"","operations":[{"httpMethod":"GET","nickname":"查询","type":"","summary":"查找指定插件类型信息","parameters":[{"paramType":"path","name":"typeid","description":"\"要查询的插件类型ID\"","dataType":"string","type":"","format":"","allowMultiple":false,"required":true,"minimum":0,"maximum":0}],"responseMessages":[{"code":200,"message":"models.PluginType","responseModel":""},{"code":403,"message":"PluginTypeID is empty","responseModel":""}]}]},{"path":"/","description":"","operations":[{"httpMethod":"GET","nickname":"查询所有","type":"","summary":"获取插件类型列表","responseMessages":[{"code":200,"message":"models.PluginTypes","responseModel":""}]}]},{"path":"/:typeid","description":"","operations":[{"httpMethod":"DELETE","nickname":"删除","type":"","summary":"删除指定插件类型","parameters":[{"paramType":"path","name":"typeid","description":"\"要删除的插件类型ID\"","dataType":"string","type":"","format":"","allowMultiple":false,"required":true,"minimum":0,"maximum":0}],"responseMessages":[{"code":200,"message":"Success","responseModel":""},{"code":403,"message":"PluginTypeID is empty","responseModel":""}]}]}]},"/project":{"apiVersion":"","swaggerVersion":"1.2","basePath":"","resourcePath":"/project","produces":["application/json","application/xml","text/plain","text/html"],"apis":[{"path":"/","description":"","operations":[{"httpMethod":"POST","nickname":"创建","type":"","summary":"创建项目","parameters":[{"paramType":"body","name":"body","description":"\"客户端信息JSON实体\"","dataType":"Project","type":"","format":"","allowMultiple":false,"required":true,"minimum":0,"maximum":0}],"responseMessages":[{"code":200,"message":"models.Project.projectid","responseModel":""},{"code":403,"message":"body is empty","responseModel":""}]}]},{"path":"/:projectid","description":"","operations":[{"httpMethod":"GET","nickname":"查询","type":"","summary":"查找指定项目信息","parameters":[{"paramType":"path","name":"projectid","description":"\"要查询的项目ID\"","dataType":"string","type":"","format":"","allowMultiple":false,"required":true,"minimum":0,"maximum":0}],"responseMessages":[{"code":200,"message":"models.Project","responseModel":""},{"code":403,"message":"ProjectID is empty","responseModel":""}]}]},{"path":"/","description":"","operations":[{"httpMethod":"GET","nickname":"查询所有","type":"","summary":"获取项目列表","responseMessages":[{"code":200,"message":"models.Projects","responseModel":""}]}]},{"path":"/:projectid","description":"","operations":[{"httpMethod":"DELETE","nickname":"删除","type":"","summary":"删除指定项目","parameters":[{"paramType":"path","name":"projectid","description":"\"要删除的项目ID\"","dataType":"string","type":"","format":"","allowMultiple":false,"required":true,"minimum":0,"maximum":0}],"responseMessages":[{"code":200,"message":"Success","responseModel":""},{"code":403,"message":"ProjectID is empty","responseModel":""}]}]}]}}`
var rootapi swagger.ResourceListing

var apilist map[string]*swagger.ApiDeclaration

func init() {
	basepath := "/v1"
	err := json.Unmarshal([]byte(rootinfo), &rootapi)
	if err != nil {
		beego.Error(err)
	}
	err = json.Unmarshal([]byte(subapi), &apilist)
	if err != nil {
		beego.Error(err)
	}
	beego.GlobalDocApi["Root"] = rootapi
	for k, v := range apilist {
		for i, a := range v.Apis {
			a.Path = urlReplace(k + a.Path)
			v.Apis[i] = a
		}
		v.BasePath = basepath
		beego.GlobalDocApi[strings.Trim(k, "/")] = v
	}
}


func urlReplace(src string) string {
	pt := strings.Split(src, "/")
	for i, p := range pt {
		if len(p) > 0 {
			if p[0] == ':' {
				pt[i] = "{" + p[1:] + "}"
			} else if p[0] == '?' && p[1] == ':' {
				pt[i] = "{" + p[2:] + "}"
			}
		}
	}
	return strings.Join(pt, "/")
}
