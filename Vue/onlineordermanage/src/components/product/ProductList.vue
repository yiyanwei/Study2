<template>
  <div>
    <header>
      <el-form :inline="true" :model="form" class="demo-form-inline">
        <el-form-item label="产品名称">
          <el-input v-model="form.categoryName" placeholder="产品名称"></el-input>
        </el-form-item>
        <el-form-item label="产品类别">
          <el-cascader
            :options="procateoptions"
            :props="{ checkStrictly: true,emitPath:false }"
            clearable
            style="width:100%;"
          ></el-cascader>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="onSearch">查询</el-button>
          <el-button type="primary" @click="dialogAddProduct = true">新增</el-button>
        </el-form-item>
      </el-form>
    </header>
    <el-table :data="tableData" style="width: 100%">
      <el-table-column type="expand">
        <template slot-scope="props">
          <el-form label-position="left" inline class="demo-table-expand">
            <el-form-item label="产品名称">
              <span>{{ props.row.proName }}</span>
            </el-form-item>
            <el-form-item label="产品编码">
              <span>{{ props.row.proCode }}</span>
            </el-form-item>
            <el-form-item label="产品类别">
              <span>{{ props.row.categoryName }}</span>
            </el-form-item>
            <el-form-item label="产品基本单位">
              <span>{{ props.row.proBaseUnit }}</span>
            </el-form-item>
            <el-form-item label="创建时间">
              <span>{{ props.row.creationTime }}</span>
            </el-form-item>
            <el-form-item label="产品描述">
              <span>{{ props.row.proDesc }}</span>
            </el-form-item>
          </el-form>
        </template>
      </el-table-column>
      <el-table-column label="产品名称" prop="proName"></el-table-column>
      <el-table-column label="产品编码" prop="proCode"></el-table-column>
      <el-table-column label="产品类别" prop="categoryName"></el-table-column>
      <el-table-column label="产品基本单位" prop="proBaseUnit"></el-table-column>
      <el-table-column label="创建时间" prop="creationTime"></el-table-column>
      <el-table-column label="操作人" prop="realName"></el-table-column>
      <el-table-column fixed="right" label="操作" width="100">
        <template slot-scope="scope">
          <!-- <el-button @click="handleClick(scope.row)" type="text" size="small">查看</el-button> -->
          <el-button type="text" size="small" @click="onEdit(scope.row)">编辑</el-button>
        </template>
      </el-table-column>
    </el-table>
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
      title="新增产品"
      :visible.sync="dialogAddProduct"
      width="30%"
      :close-on-click-modal="false"
      :destroy-on-close="true"
    >
      <AddProduct v-if="dialogAddProduct" />
    </el-dialog>
  </div>
</template>
<style>
.demo-table-expand {
  font-size: 0;
}
.demo-table-expand label {
  width: 100px;
  color: #99a9bf;
}
.demo-table-expand .el-form-item {
  margin-right: 0;
  margin-bottom: 0;
  width: 100%;
}
</style>
<script>
import AddProduct from "./AddPro.vue";
import http from "../../common/http/vueresource.js";
export default {
  data() {
    return {
      dialogAddProduct:false,
      tableData: [],
      procateoptions: [],
      total: 0,
      form: {
        pageIndex: 0,
        pageSize: 10
      }
    };
  },
  components:{
    AddProduct
  },
  mounted() {
    this.getCategoryList();
    this.getData();
  },
  methods: {
    onSearch(){

    },
    onEdit(row) {
      console.log(row);
    },
    currentChange(pageIndex){
      this.form.pageIndex = pageIndex;
      this.getData();
    },
    getCategoryList() {
      var api = "/ProCategory/GetDropDownListAsync";
      http.get(api, null, response => {
        if (response && response.success && response.data) {
          this.procateoptions = response.data;
        } else {
          this.procateoptions.clear();
        }
      });
    },
    getData() {
      //请求查询数据
      var api = "/ProInfo/SearchPageList";
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