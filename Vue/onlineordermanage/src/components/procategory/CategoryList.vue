<template>
  <div>
    <header>
      <el-form :inline="true" :model="form" class="demo-form-inline">
        <el-form-item label="分类名称">
          <el-input v-model="form.categoryName" placeholder="分类名称"></el-input>
        </el-form-item>
        <el-form-item label="父级分类名称">
          <el-input v-model="form.parentCategoryName" placeholder="父级分类名称"></el-input>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="onSearch">查询</el-button>
          <el-button type="primary" @click="dialogAddCategory = true">新增</el-button>
        </el-form-item>
      </el-form>
    </header>
    <div>
      <el-table ref="multipleTable" :data="tableData" tooltip-effect="dark" style="width: 100%">
        <el-table-column type="selection" width="55"></el-table-column>
        <el-table-column prop="categoryName" label="分类名称"></el-table-column>
        <el-table-column prop="parentCategoryName" label="父级分类名称"></el-table-column>
        <el-table-column prop="creationTime" label="创建时间"></el-table-column>
        <el-table-column prop="realName" label="操作人" show-overflow-tooltip></el-table-column>
        <el-table-column fixed="right" label="操作" width="100">
          <template slot-scope="scope">
            <!-- <el-button @click="handleClick(scope.row)" type="text" size="small">查看</el-button> -->
            <el-button type="text" size="small" @click="onEdit(scope.row)">编辑</el-button>
          </template>
        </el-table-column>
      </el-table>
    </div>
    <div>
      <el-pagination
        background
        layout="prev, pager, next"
        :total="total"
        :page-size="form.pageSize"
        @current-change="currentChange"
      ></el-pagination>
    </div>
    <el-dialog
      title="新增产品分类"
      :visible.sync="dialogAddCategory"
      width="350px"
      :close-on-click-modal="false"
      :destroy-on-close="true"
    >
      <AddCategory />
    </el-dialog>

    <el-dialog
      title="修改产品分类"
      :visible.sync="dialogEditCategory"
      width="350px"
      :close-on-click-modal="false"
      :destroy-on-close="true"
    >
      <EditCategory v-if="dialogEditCategory" ref="editCategory" />
    </el-dialog>
  </div>
</template>
<script>
//引入新增组件
import AddCategory from "./AddCategory.vue";
import EditCategory from "./EditCategory.vue";
import http from "../../common/http/vueresource.js";
export default {
  data() {
    return {
      currentId: "",
      dialogAddCategory: false,
      dialogEditCategory: false,
      form: {
        pageIndex: 0,
        pageSize: 10,
        categoryName: "",
        parentCategoryName: ""
      },
      total: 0,
      tableData: []
    };
  },
  components: {
    AddCategory,
    EditCategory
  },
  mounted: function() {
    this.initData();
  },
  methods: {  
    initData() {
      this.getData();
    },
    currentChange(pageIndex) {
      this.form.pageIndex = pageIndex;
      this.getData();
    },
    onSearch() {
      this.form.pageIndex = 0;
      this.getData();
    },
    onEdit(row) {
      this.currentId = row.id;
      this.dialogEditCategory = true;
    },
    getData() {
      //请求查询数据
      var api = "/ProCategory/SearchPageList";
      //this.form.pageIndex = pIndex;
      http.get(api, JSON.stringify(this.form), response => {
        if (response && response.success && response.data) {
          //绑定列表
          this.tableData = response.data.items;
          //总记录数
          this.total = response.data.totalCount;
        } else {
          this.tableData.clear();
        }
      });
    }
  }
};
</script>