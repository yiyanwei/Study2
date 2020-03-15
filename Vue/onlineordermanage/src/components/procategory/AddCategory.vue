<template>
  <el-form ref="form" :model="form" label-width="80px">
    <el-form-item label="父级分类">
      <el-cascader
        v-model="form.ParentId"
        :options="form.procateoptions"
        :props="{ checkStrictly: true,emitPath:false }"
        clearable
        style="width:100%;"
      ></el-cascader>
    </el-form-item>
    <el-form-item label="分类名称">
      <el-input v-model="form.CategoryName"></el-input>
    </el-form-item>
    <el-form-item>
      <el-button type="primary" @click="onSubmit">添加</el-button>
      <el-button type="primary" @click="onCancel">取消</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
import http from "../../common/http/vueresource.js";
export default {
  data() {
    return {
      form: {
        procateoptions: [],
        ParentId: "",
        CategoryName: ""
      }
    };
  },
  mounted: function() {
    this.initData();
  },
  methods: {
    initData() {
      var api = "/ProCategory/GetDropDownListAsync";
      http.get(api, null, response => {
        if (response && response.success && response.data) {
          this.form.procateoptions = response.data;
        } else {
          this.form.procateoptions.clear();
        }
      });
    },
    onSubmit() {
      var api = "/ProCategory/Add";
      var data = {
        ParentId: this.form.ParentId,
        CategoryName: this.form.CategoryName
      };
      http.post(api, data, response => {
        if (response.success) {
          this.$message({
            message: "产品分类添加成功",
            type: "success"
          });
          //获取父级窗口是否正确
          if (this.$parent.$parent) {
            //关闭窗口
            this.$parent.$parent.dialogAddCategory = false;
            this.$parent.$parent.getData();
          }
        } else {
          this.$message.error(response.errMsg);
        }
      });
    },
    onCancel() {
      if (this.$parent.$parent) {
        this.$parent.$parent.dialogAddCategory = false;
      }
    }
  }
};
</script>