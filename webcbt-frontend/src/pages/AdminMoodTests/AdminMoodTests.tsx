import './AdminMoodTests.css';
import {Button, Table} from 'antd';
import {useLayoutEffect, useState} from 'react';
import {isDev} from '../../config';
import mockMoodTests from '../../helpers/mockMoodTests';
import {useGetAllMoodTestsMutation} from '../../store/services/evaluation';
import {MoodTestRequest} from '../../types/MoodTest';

const columns = [
  {
    title: 'User ID',
    dataIndex: 'userId',
    key: 'userId',
  },
  {
    title: 'Category',
    dataIndex: 'category',
    key: 'category',
  },
  {
    title: 'Question 1',
    dataIndex: 'question1',
    key: 'question1',
  },
  {
    title: 'Question 2',
    dataIndex: 'question2',
    key: 'question2',
  },
  {
    title: 'Question 3',
    dataIndex: 'question3',
    key: 'question3',
  },
  {
    title: 'Question 4',
    dataIndex: 'question4',
    key: 'question4',
  },
  {
    title: 'Question 5',
    dataIndex: 'question5',
    key: 'question5',
  },
  {
    key: 'delete',
    render: () => (
      <Button type="text" danger>
        Delete
      </Button>
    ),
  },
];

const AdminMoodTests = () => {
  const [getAllMoodTests] = useGetAllMoodTestsMutation();
  let [allMoodTests, setAllMoodTests] = useState<MoodTestRequest[]>([]);

  useLayoutEffect(() => {
    const fetchAllMoodTests = async () => {
      setAllMoodTests(await getAllMoodTests().unwrap());
    };
    fetchAllMoodTests();
  }, [getAllMoodTests]);

  return (
    <Table
      className="adminMoodTestsTable"
      dataSource={isDev ? mockMoodTests : allMoodTests}
      columns={columns}
    />
  );
};

export default AdminMoodTests;