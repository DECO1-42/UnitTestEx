using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Reflection;
using UnitTestEx;
using Assert = NUnit.Framework.Assert;

namespace UnitTestProject
{
    /// <summary>
    /// Summary description for FileStorageTest
    /// </summary>
    [TestClass]
    public class FileStorageTest
    {
        public const string MAX_SIZE_EXCEPTION = "DIFFERENT MAX SIZE";
        public const string NULL_FILE_EXCEPTION = "NULL FILE";
        public const string NO_EXPECTED_EXCEPTION_EXCEPTION = "There is no expected exception";

        public const string SPACE_STRING = " ";
        public const string FILE_PATH_STRING = "@D:\\JDK-intellij-downloader-info.txt";
        public const string CONTENT_STRING = "Some text";
        public const string REPEATED_STRING = "AA";
        public const string WRONG_SIZE_CONTENT_STRING = "TEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtext";
        public const string TIC_TOC_TOE_STRING = "tictoctoe.game";

        public const int NEW_SIZE = 5;

        public FileStorage storage = new FileStorage(NEW_SIZE);

        /* ПРОВАЙДЕРЫ */

        static object[] NewFilesData =
        {
            new object[] { new File(REPEATED_STRING, CONTENT_STRING) },
            new object[] { new File(SPACE_STRING, WRONG_SIZE_CONTENT_STRING) },
            new object[] { new File(FILE_PATH_STRING, CONTENT_STRING) }
        };

        static object[] FilesForDeleteData =
        {
            new object[] { new File(REPEATED_STRING, CONTENT_STRING), REPEATED_STRING },
            new object[] { null, TIC_TOC_TOE_STRING }
        };

        static object[] NewExceptionFileData = {
            new object[] { new File(REPEATED_STRING, CONTENT_STRING) }
        };

        /* Тестирование записи файла */
        [Test, TestCaseSource(nameof(NewFilesData))]//
        public void WriteTest(File file)
        {
            Assert.True(storage.Write(file), "Cannot write");
            storage.DeleteAllFiles();
        }

        /* Тестирование записи дублирующегося файла */
        [Test, TestCaseSource(nameof(NewExceptionFileData))]
        public void WriteExceptionTest(File file)// 
        {
            bool isException = false;
            try
            {
                storage.Write(file);
                Assert.False(storage.Write(file), "fd");
                storage.DeleteAllFiles();
            }
            catch (FileNameAlreadyExistsException)
            {
                isException = true;
            }
            Assert.True(isException, NO_EXPECTED_EXCEPTION_EXCEPTION);
        }

        /* Тестирование проверки существования файла */
        [Test, TestCaseSource(nameof(NewFilesData))]//
        public void IsExistsTest(File file)
        {
            storage.Write(file);
            String name = file.GetFilename();
            Assert.True(storage.IsExists(name), $"File{name} desen't exists");
            storage.DeleteAllFiles();
        }

        /* Тестирование удаления файла */
        [Test, TestCaseSource(nameof(FilesForDeleteData))]
        public void DeleteTest(File file, String fileName) {
            storage.Write(file);
            Assert.True(storage.Delete(fileName));
        }

        /* Тестирование получения файлов */
        [Test]
        public void GetFilesTest()//
        {
            foreach (File el in storage.GetFiles())
            {
                Assert.NotNull(el, "Files does not exist");
            }
        }

        // Почти эталонный
        /* Тестирование получения файла */
        [Test, TestCaseSource(nameof(NewFilesData))]
        public void GetFileTest(File expectedFile)
        {
            storage.Write(expectedFile);
            bool isException = false;
            try
            {
                File actualfile = storage.GetFile(expectedFile.GetFilename());
            }
            catch
            {
                isException = true;
            }
            finally
            {
                storage.DeleteAllFiles();
            }
            Assert.True(!isException, "GetFileFailed");
        }
        //Тест на корректность вычисления размера пустого файла
        [Test]
        public void GetSize_EmptyContent_ReturnZero()
        {
            var file = new File("empty.txt", string.Empty);
            var size = file.GetSize();
            Assert.AreEqual(0, size, "Size empty file should be = 0");
        }
        //Тест на удаление несуществующего файла
        [Test]
        public void DeleteNonExistentFile_ReturnsFalse()
        {
            var storage = new FileStorage();
            var fileName = "nonex.txt";
            var result = storage.Delete(fileName);
            Assert.False(result, "Attempt to delete a non-existent file");
        }
        [Test]
        //Тест на получение несуществующего файла
        public void GetFile_NonExistingFile_ShouldReturnNull()
        {
            FileStorage storage = new FileStorage();
            var fileName = "test.txt";
            var result = storage.GetFile(fileName);
            Assert.Null(result, "Method should return null if file non exists");
        }
    }
}
