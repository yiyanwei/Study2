<template>
  <el-form ref="ruleForm" :model="form" :rules="rules" label-width="100px">
    <el-form-item label="产品名称" prop="proName">
      <el-input v-model="form.proName" placeholder="请输入产品名称" style="width:80%;"></el-input>
    </el-form-item>
    <el-form-item label="产品编码">{{this.form.proCode}}</el-form-item>
    <el-form-item label="产品分类" prop="categoryId">
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
    <el-form-item label="产品图片" prop="uploadId">
      <el-upload
        :action="uploadAddr"
        :data="data"
        name="files"
        :file-list="fileList"
        list-type="picture-card"
        :auto-upload="true"
        :limit="1"
        :v-model="form.uploadId"
        :on-success="uploadSuccess"
        :on-exceed="uploadExceed"
        :on-preview="handlePictureCardPreview"
        :on-remove="handleRemove"
      >
        <i class="el-icon-plus"></i>
      </el-upload>
      <el-dialog :visible.sync="dialogVisible" :modal="false">
        <img width="100%" :src="dialogImageUrl" alt />
      </el-dialog>
    </el-form-item>
    <el-form-item label="产品描述">
      <el-input type="textarea" v-model="form.proDesc"></el-input>
    </el-form-item>
    <el-form-item>
      <el-button type="primary" @click="submitForm('ruleForm')">立即创建</el-button>
      <el-button @click="onCancel">取消</el-button>
    </el-form-item>
  </el-form>
</template>
<script>
import http from "../../common/http/vueresource.js";
export default {
  data() {
    return {
      uploadAddr: http.rootApi + "/upload/UploadImageAndGenerateThum",
      fit: "contain",
      url: "",
      dialogImageUrl: "",
      dialogVisible: false,
      disabled: false,
      uploadVisible: true,
      procateoptions: [],
      data: {
        limitSize: 100
      },
      fileList: [],
      form: {
        id: "",
        proName: "",
        proCode: "",
        proBaseUnit: "",
        categoryId: "",
        uploadId: "",
        proDesc: "",
        rowVersion: ""
      },
      rules: {
        proName: [
          { required: true, message: "请输入产品名称", trigger: "blur" }
        ],
        categoryId: [
          {
            required: true,
            message: "请选择产品分类",
            trigger: "change"
          }
        ],
        uploadId: [
          {
            required: true,
            message: "请上传图片",
            trigger: "change"
          }
        ]
      }
    };
  },
  mounted() {
    //加载分类信息
    this.getCategoryList();
    this.form.id = this.$parent.$parent.currentId;
    //加载详情
    this.getProInfo();
  },
  methods: {
    uploadSuccess(data) {
      if (data.data && data.data.uploadId) {
        this.form.uploadId = data.data.uploadId;
        
        this.$refs["ruleForm"].validate();
      }
    },
    uploadExceed() {
      this.$message("只能上传一张图片");
    },
    getProInfo() {
      //获取分类详情信息
      var getApi = "/ProInfo/GetProInfo";
      var data = {
        id: this.form.id
      };
      http.get(getApi, JSON.stringify(data), response => {
        if (response.success) {
          //赋值
          this.form.proName = response.data.proName;
          this.form.proCode = response.data.proCode;
          this.form.proBaseUnit = response.data.proBaseUnit;
          this.form.categoryId = response.data.categoryId;
          this.form.proDesc = response.data.proDesc;
          this.form.rowVersion = response.data.rowVersion;
          this.fileList = response.data.fileInfos;
          for (var key in this.fileList) {
            this.fileList[key].url =
              "http://localhost:5002" + this.fileList[key].url;
          }
        } else {
          //弹出错误信息提示
          this.$message.error(response.errMsg);
        }
      });
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
    handleRemove() {
      this.form.uploadId = "";
      this.$refs["ruleForm"].validate();
    },
    handlePictureCardPreview(file) {
      this.dialogImageUrl = file.url;
      this.dialogVisible = true;
    },
    submitForm(formName) {
      this.$refs[formName].validate(valid => {
        //验证通过
        if (valid) {
          var api = "/ProInfo/Add";
          http.post(api, this.form, response => {
            if (response.success) {
              this.$message({
                message: "产品保存成功",
                type: "success"
              });
              //获取父级窗口是否正确
              if (this.$parent.$parent) {
                //关闭窗口
                this.$parent.$parent.dialogAddProduct = false;
                this.$parent.$parent.getData();
              }
            } else {
              this.$message.error(response.errMsg);
            }
          });
        } else {
          return false;
        }
      });
    },
    onCancel() {
      if (this.$parent.$parent) {
        this.$parent.$parent.dialogEditProduct = false;
      }
    }
  }
};
</script>