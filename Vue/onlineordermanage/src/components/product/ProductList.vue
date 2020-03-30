<template>
  <div>
    <header>
      <el-form :inline="true" :model="form" class="demo-form-inline">
        <el-form-item label="产品名称">
          <el-input v-model="form.proName" placeholder="请输入产品名称"></el-input>
        </el-form-item>
        <el-form-item label="产品编号">
          <el-input v-model="form.proCode" placeholder="请输入产品编号" style="width:80%;"></el-input>
        </el-form-item>
        <el-form-item label="产品类别">
          <el-cascader
            v-model="form.categoryId"
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
             <el-form-item label="产品图片">
              <VueHoverMask>
            <!-- 默认插槽 -->
            <img :src="props.row" alt class="el-upload-list__item-thumbnail" />
            <template v-slot:action>
              <i class="el-icon-zoom-in" @click="handlePicturePreview" :data-pid="item.id"></i>
            </template>
          </VueHoverMask>
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
      style="z-index:2"
      :visible.sync="dialogAddProduct"
      width="30%"
      :close-on-click-modal="false"
      :destroy-on-close="true"
    >
      <AddProduct v-if="dialogAddProduct" />
    </el-dialog>
    <el-dialog
      title="编辑产品"
      style="z-index:3"
      width="30%"
      :close-on-click-modal="false"
      :visible.sync="dialogEditProduct"
    >
      <EditProduct v-if="dialogEditProduct" />
    </el-dialog>

    <el-dialog
      :width="imgdialogWidth"
      :visible.sync="imgdialogVisible"
      :close-on-click-modal="false"
      :modal="true"
      :destroy-on-close="true"
    >
      <img :src="imageUrl" v-if="imgdialogVisible" v-on:load.stop="imgOnload" alt />
    </el-dialog>
  </div>
</template>
<style>
.el-upload-list .el-upload-list__item .vue-hover-mask {
  float: left;
  margin-left: -80px;
  height: 80px;
  width: 80px;
  margin-top: -5px;
}

.el-upload-list
  .el-upload-list__item
  .vue-hover-mask
  img.el-upload-list__item-thumbnail {
  position: absolute;
  left: 0;
  right: 0;
  top: 0;
  bottom: 0;
  margin: auto;
  height: auto;
  width: auto;
  float: none;
}

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
import VueHoverMask from "vue-hover-mask";
import AddProduct from "./AddPro.vue";
import EditProduct from "./EditPro.vue";
import http from "../../common/http/vueresource.js";
export default {
  data() {
    return {
      imgdialogWidth: "0px",
      imgdialogVisible: false,
      imageUrl: "",
      currentId: "",
      dialogAddProduct: false,
      dialogEditProduct: false,
      tableData: [],
      procateoptions: [],
      total: 0,
      form: {
        pageIndex: 0,
        pageSize: 10,
        proName: "",
        proCode: "",
        categoryId: ""
      }
    };
  },
  components: {
    AddProduct,
    EditProduct,
    VueHoverMask
  },
  mounted() {
    this.getCategoryList();
    this.getData();
  },
  methods: {
    onSearch() {

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
    },
    imgOnload(e) {
      if (e.path && e.path[0] && e.path[0].nodeName == "IMG") {
        this.imgdialogWidth = (e.path[0].width + 40) + "px";
      }
    },
    onEdit(row) {
      this.currentId = row.id;
      this.dialogEditProduct = true;
    },
    currentChange(pageIndex) {
      this.form.pageIndex = pageIndex;
      this.getData();
    }
  }
};
</script>