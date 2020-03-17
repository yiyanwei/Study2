<template>
  <el-form ref="form" :model="form" label-width="100px">
    <el-form-item label="产品名称">
      <el-input v-model="form.proName" placeholder="请输入产品名称" style="width:80%;"></el-input>
    </el-form-item>
    <el-form-item label="产品编码">
      <el-input v-model="form.proCode" placeholder="请输入产品编码" style="width:80%;"></el-input>
    </el-form-item>
    <el-form-item label="产品分类">
      <el-cascader
        placeholder="请选择产品分类"
        v-model="form.categoryId"
        :options="procateoptions"
        :props="{ checkStrictly: true,emitPath:false }"
        clearable
        style="width:80%;"
      ></el-cascader>
    </el-form-item>
    <el-form-item label="产品基本单位">
      <el-input v-model="form.proBaseUnit" style="width:80%;"></el-input>
    </el-form-item>
    <el-form-item label="产品图片">
      <el-upload v-if="uploadVisible"
        action="https://jsonplaceholder.typicode.com/posts/"
        list-type="picture-card"
        :limit="1"
        :on-success="uploadSuccess"
        :on-preview="handlePictureCardPreview"
        :on-remove="handleRemove"
      >
        <i class="el-icon-plus"></i>
      </el-upload>
      <el-dialog :visible.sync="dialogVisible">
        <img width="100%" :src="dialogImageUrl" alt />
      </el-dialog>
    </el-form-item>
    <el-form-item label="产品描述">
      <el-input type="textarea" v-model="form.proDesc"></el-input>
    </el-form-item>
    <el-form-item>
      <el-button type="primary" @click="onSubmit">立即创建</el-button>
      <el-button>取消</el-button>
    </el-form-item>
  </el-form>
</template>
<script>
import http from "../../common/http/vueresource.js";
export default {
  data() {
    return {
      uploadVisible:true,
      procateoptions: [],
      form: {
        proName: "",
        proCode: "",
        proBaseUnit: "",
        categoryId: "",
        proDesc:""
      }
    };
  },
  mounted(){
    this.getCategoryList();
  },
  methods: {
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
    uploadSuccess(a){
      console.log(a);
    },
    // handlePictureCardPreview(a){
    //   console.log(a);
    //   return a;
    // },
    handleRemove(a){
      console.log(a);
    },
    onSubmit() {
      console.log("submit!");
    }
  }
};
</script>