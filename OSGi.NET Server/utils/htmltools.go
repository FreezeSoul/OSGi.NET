package utils

import (
	"html/template"

	"github.com/astaxie/beego"
)

var index = 0

func add(x, y int) int {
	return x + y
}

func rowIndex(flag bool) (out int) {
	if flag == true {
		index = index + 1
	}
	return index
}

func isActive(flagName string, currentName string) template.HTMLAttr {
	if flagName == currentName {
		return template.HTMLAttr("active")
	}
	return template.HTMLAttr("")
}

func isSelected(flagValue string, currentValue string) template.HTMLAttr {
	if flagValue == currentValue {
		return template.HTMLAttr("selected")
	}
	return template.HTMLAttr("")
}

func RawHTML(text string) template.HTML {
	return template.HTML(text)
}

func RawHTMLAttr(text string) template.HTMLAttr {
	return template.HTMLAttr(text)
}

func RawCSS(text string) template.CSS {
	return template.CSS(text)
}

func RawJS(text string) template.JS {
	return template.JS(text)
}

func RawJSStr(text string) template.JSStr {
	return template.JSStr(text)
}

func RawURL(text string) template.URL {
	return template.URL(text)
}

func init() {
	beego.AddFuncMap("add", add)
	beego.AddFuncMap("index", rowIndex)
	beego.AddFuncMap("isactive", isActive)
	beego.AddFuncMap("isselected", isSelected)
	beego.AddFuncMap("rawhtml", RawHTML)
}
