<template>
  <div>
    <header></header>
    <div>
      <el-table ref="multipleTable" :data="tableData" tooltip-effect="dark" style="width: 100%">
        <el-table-column type="selection" width="55"></el-table-column>
        <el-table-column prop="categoryName" label="分类名称"></el-table-column>
        <el-table-column prop="parentCategoryName" label="父级分类名称"></el-table-column>
        <el-table-column prop="creationTime" label="创建时间"></el-table-column>
        <el-table-column prop="realName" label="操作人" show-overflow-tooltip></el-table-column>
      </el-table>
    </div>
    <div>
      <el-pagination
        background
        layout="prev, pager, next"
        :total="total"
        :page-size="5"
        @current-change="currentChange"
      ></el-pagination>
    </div>
  </div>
</template>
<script>
import http from "../../common/http/vueresource.js";
export default {
  data() {
    return {
      total: 0,
      tableData: []
    };
  },
  mounted: function() {
    this.initData();
  },
  methods: {
    currentChange(pageIndex) {
      this.getData(pageIndex);
    },
    getData(pIndex) {
      //请求查询数据
      var api = "/ProCategory/SearchPageList";
      var data = {
        pageIndex: pIndex,
        pageSize: 5,
        categoryName: "",
        parentId: "",
        parentCategoryName: ""
      };
      http.get(api, JSON.stringify(data), response => {
        if (response && response.success && response.data) {
          //绑定列表
          this.tableData = response.data.items;
          //总记录数
          this.total = response.data.totalCount;
        } else {
          this.tableData.clear();
        }
      });
    },
    initData() {
      this.getData(0);
    }
  }
};
</script>