package helper

import (
	"os"
	"strings"
)

func GetSeparator() string {
	var path string
	if os.IsPathSeparator('\\') { //前边的判断是否是系统的分隔符
		path = "\\"
	} else {
		path = "/"
	}
	return path
}

func CreatDir(path string) error {
	sep := GetSeparator()
	dir, _ := os.Getwd() //当前的目录
	fullpath := dir + sep + path
	exist, _ := ExistsDir(fullpath)
	if exist {
		return nil
	}
	err := os.Mkdir(fullpath, os.ModePerm) //在当前目录下生成md目录
	return err
}

func CreatDirAll(path string, flag string) error {
	pathArray := strings.Split(path, flag)
	sep := GetSeparator()
	var partialPath string
	for _, element := range pathArray {
		partialPath = partialPath + element + sep
	}
	dir, _ := os.Getwd() //当前的目录
	fullpath := dir + sep + partialPath
	exist, _ := ExistsDir(fullpath)
	if exist {
		return nil
	}
	err := os.MkdirAll(fullpath, os.ModePerm) //在当前目录下生成md目录
	return err
}

func ExistsDir(path string) (bool, error) {
	_, err := os.Stat(path)
	if err == nil {
		return true, nil
	}
	if os.IsNotExist(err) {
		return false, nil
	}
	return false, err
}
