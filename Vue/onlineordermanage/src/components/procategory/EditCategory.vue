<template>
  <el-form ref="form" :model="form" label-width="80px">
    <el-form-item label="父级分类">
      <el-cascader
        v-model="form.parentId"
        :options="procateoptions"
        :props="{ checkStrictly: true,emitPath:false }"
        clearable
        style="width:100%;"
      ></el-cascader>
    </el-form-item>
    <el-form-item label="分类名称">
      <el-input v-model="form.categoryName"></el-input>
    </el-form-item>
    <el-form-item>
      <el-button type="primary" @click="onSubmit">保存</el-button>
      <el-button type="primary" @click="onCancel">取消</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
import http from "../../common/http/vueresource.js";
export default {
  data() {
    return {
      procateoptions: [],
      form: {
        id: "",
        parentId: "",
        categoryName: "",
        rowVersion: ""
      }
    };
  },
  mounted: function() {
    this.form.id = this.$parent.$parent.currentId;
    this.initData();
  },
  methods: {
    getCategory() {
      //获取分类详情信息
      var getApi = "/ProCategory/GetEntityById";
      var data = {
        id: this.form.id
      };
      http.get(getApi, JSON.stringify(data), response => {
        if (response.success) {
          //赋值
          this.form.parentId = response.data.parentId;
          this.form.categoryName = response.data.categoryName;
          this.form.rowVersion = response.data.rowVersion;
        } else {
          //弹出错误信息提示
          this.$message.error(response.errMsg);
        }
      });
    },
    initData() {
      var api = "/ProCategory/GetDropDownListAsync";
      http.get(api, null, response => {
        if (response && response.success && response.data) {
          this.procateoptions = response.data;
        } else {
          this.procateoptions.clear();
        }
        //获取当前更新记录的数据
        this.getCategory();
      });
    },
    onSubmit() {
      var api = "/ProCategory/Edit";
      http.post(api, this.form, response => {
        if (response.success) {
          this.$message({
            message: "产品分类保存成功",
            type: "success"
          });
          //获取父级窗口是否正确
          if (this.$parent.$parent) {
            //关闭窗口
            this.$parent.$parent.dialogEditCategory = false;
            this.$parent.$parent.getData();
          }
        } else {
          this.$message.error(response.errMsg);
        }
      });
    },
    onCancel() {
      if (this.$parent.$parent) {
        this.$parent.$parent.dialogEditCategory = false;
      }
    }
  }
};
</script>