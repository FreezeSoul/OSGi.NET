package helper

import (
	"strconv"
)

type Paging interface {
	SetCurrentPage(int)
	CurrentPage() int
	LineSize() int
	SetTotalPage()
	TotalPage() int
	TotalCount() int
	GetPageData(int) (string, string, *[]string)
}

type paging struct {
	currentPage int // 当前页
	lineSize    int // 每页几行
	totalPage   int // 总页数
	totalCount  int // 总行数
}

func NewPaging(lineSize, totalCount int) Paging {
	return &paging{
		lineSize:   lineSize,
		totalCount: totalCount,
	}
}

func (p *paging) SetCurrentPage(currentPage int) {
	p.currentPage = currentPage
}

func (p paging) CurrentPage() int {
	return p.currentPage
}

func (p paging) LineSize() int {
	return p.lineSize
}

func (p *paging) SetTotalPage() {
	totalPage := p.TotalCount() / p.LineSize()
	if p.TotalCount()%p.LineSize() == 0 {
		p.totalPage = totalPage
	} else {
		p.totalPage = totalPage + 1
	}
}

func (p paging) TotalPage() int {
	return p.totalPage
}

func (p paging) TotalCount() int {
	return p.totalCount
}

func (p paging) GetPageData(pageindex int) (string, string, *[]string) {
	pages := make([]string, p.TotalPage())
	for i := 1; i <= p.TotalPage(); i++ {
		pages[i-1] = strconv.Itoa(i)
	}
	if len(pages) == 0 {
		pages = make([]string, 1)
		pages[0] = "1"
	}

	prevpage := pageindex - 1
	if prevpage < 1 {
		prevpage = 1
	}

	nextpage := pageindex + 1
	if nextpage > p.TotalPage() {
		nextpage = pageindex
	}
	return strconv.Itoa(prevpage), strconv.Itoa(nextpage), &pages
}
