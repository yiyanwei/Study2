<template>
  <div>
    <header></header>
    <div>
      <el-table ref="multipleTable" :data="tableData" tooltip-effect="dark" style="width: 100%">
        <el-table-column type="selection" width="55"></el-table-column>
        <el-table-column prop="categoryName" label="分类名称"></el-table-column>
        <el-table-column prop="parentCategoryName" label="父级分类名称"></el-table-column>
        <el-table-column prop="creationTime" label="创建时间" show-overflow-tooltip></el-table-column>
      </el-table>
    </div>
    <div>
      <el-pagination background layout="prev, pager, next" :total="1000"></el-pagination>
    </div>
  </div>
</template>
<script>
import http from "../../common/http/vueresource.js";
export default {
  data() {
    return {
      tableData: []
    };
  },
  mounted: function() {
    this.initData();
  },
  methods: {
    initData() {
      var api = "/ProCategory/SearchPageList";
      var data = {
        pageIndex: 0,
        pageSize: 10,
        categoryName: "",
        parentId: "",
        parentCategoryName: ""
      };
      http.get(api, JSON.stringify(data), response => {
        if (response && response.success && response.data) {
          this.tableData = response.data.items;
          console.log(this.tableData);
        } else {
          this.tableData.clear();
        }
      });
    }
  }
};
</script>